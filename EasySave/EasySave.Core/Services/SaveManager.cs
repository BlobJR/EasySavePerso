using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasySave.Core.Models;

namespace EasySave.Core.Services
{
    public class SaveManager
    {
        private ISave? savemethod;
        public event Action<string>? OnMessageLogged;

        // Dictionnaire contenant les threads, pauses et arrêts des jobs
        private readonly ConcurrentDictionary<string, (Thread thread, ManualResetEvent pauseEvent, CancellationTokenSource cancelToken)> activeJobs = new();
        private readonly SemaphoreSlim largeFileSemaphore = new(1, 1);
        private readonly object priorityLock = new();
        private readonly int largeFileSizeLimit = 50000; // Taille max d'un gros fichier en Ko
        private readonly List<string> priorityExtensions = new() { ".txt", ".pdf", ".docx" };


        public void StartJob(Job job)
        {
            if (activeJobs.ContainsKey(job.Name))
            {
                OnMessageLogged?.Invoke($"⚠️ Job {job.Name} is already running.");
                return;
            }

            ManualResetEvent pauseEvent = new(true);
            CancellationTokenSource cancelToken = new();

            Thread thread = new(() => PerformSave(job, pauseEvent, cancelToken.Token))
            {
                IsBackground = true
            };

            if (activeJobs.TryAdd(job.Name, (thread, pauseEvent, cancelToken)))
            {
                thread.Start();
            }
        }

        private void PerformSave(Job job, ManualResetEvent pauseEvent, CancellationToken token)
        {
            try
            {
                savemethod = job.SaveType switch
                {
                    SaveTypes.Complete => new Complete(largeFileSizeLimit, largeFileSemaphore, priorityExtensions, priorityLock),
                    SaveTypes.Differential => new Differential(largeFileSizeLimit, largeFileSemaphore, priorityExtensions, priorityLock),
                    _ => throw new InvalidOperationException("Unknown save type")
                };

                if (savemethod == null)
                {
                    OnMessageLogged?.Invoke(LanguageManager.GetString("UnknownSaveType"));
                    return;
                }

                savemethod.OnMessageLogged += message => OnMessageLogged?.Invoke(message);

                while (!token.IsCancellationRequested)
                {
                    pauseEvent.WaitOne();
                    if (token.IsCancellationRequested) break;

                    savemethod.ExecuteSave(job);
                    break;
                }
            }
            catch (Exception ex)
            {
                OnMessageLogged?.Invoke($"❌ Error in job {job.Name}: {ex.Message}");
            }
            finally
            {
                StopJob(new List<Job> { job });
            }
        }

        public void PauseJob(List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                if (activeJobs.TryGetValue(job.Name, out var jobControl))
                {
                    jobControl.pauseEvent.Reset();
                    OnMessageLogged?.Invoke($"⏸ Job {job.Name} paused.");
                }
            }
        }

        public void ResumeJob(List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                if (activeJobs.TryGetValue(job.Name, out var jobControl))
                {
                    jobControl.pauseEvent.Set();
                    OnMessageLogged?.Invoke($"▶️ Job {job.Name} resumed.");
                }
            }
        }

        public void StopJob(List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                if (activeJobs.TryRemove(job.Name, out var jobControl))
                {
                    jobControl.cancelToken.Cancel();
                    jobControl.pauseEvent.Set();
                    jobControl.thread.Join();
                    OnMessageLogged?.Invoke($"⛔ Job {job.Name} stopped.");
                }
            }
        }
    }
}
