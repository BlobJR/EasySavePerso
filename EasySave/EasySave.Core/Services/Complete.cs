using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using EasySave.Core.Models;

namespace EasySave.Core.Services
{
    public class Complete : ISave
    {
        public event Action<string>? OnMessageLogged;
        private readonly int largeFileSizeLimit;
        private readonly SemaphoreSlim largeFileSemaphore;
        private readonly List<string> priorityExtensions;
        private readonly object priorityLock;
        //private readonly ManualResetEvent pauseEvent;
        //private readonly CancellationToken token;

        public Complete(int largeFileSizeLimit, SemaphoreSlim largeFileSemaphore, List<string> priorityExtensions, object priorityLock)
        {
            this.largeFileSizeLimit = largeFileSizeLimit;
            this.largeFileSemaphore = largeFileSemaphore;
            this.priorityExtensions = priorityExtensions;
            this.priorityLock = priorityLock;
            //this.pauseEvent = pauseEvent;
            //this.token = token;
        }


        public void ExecuteSave(Job job)
        {
            OnMessageLogged?.Invoke($"📂 Starting complete save: {job.SourcePath} → {job.DestinationPath}");

            string[] files = Directory.GetFiles(job.SourcePath, "*", SearchOption.AllDirectories);
            var priorityQueue = new Queue<string>(files.Where(f => priorityExtensions.Contains(Path.GetExtension(f).ToLower())));
            var normalQueue = new Queue<string>(files.Where(f => !priorityExtensions.Contains(Path.GetExtension(f).ToLower())));
            while (priorityQueue.Count > 0 || normalQueue.Count > 0)
            {
                string file;
                bool isPriority = false;

                lock (priorityLock)
                {
                    if (priorityQueue.Count > 0)
                    {
                        file = priorityQueue.Dequeue();
                        isPriority = true;
                    }
                    else
                    {
                        file = normalQueue.Dequeue();
                    }
                }

                CopyFile(file, job, isPriority);
            }

            OnMessageLogged?.Invoke("✅ Complete save finished.");
        }

        private void CopyFile(string file, Job job, bool isPriority)
        {
            long fileSize = new FileInfo(file).Length / 1024; // Taille en Ko
            DateTime startTime = DateTime.Now;

            if (fileSize > largeFileSizeLimit)
            {
                largeFileSemaphore.Wait();
            }

            try
            {
                string relativePath = Path.GetRelativePath(job.SourcePath, file);
                string destinationFile = Path.Combine(job.DestinationPath, relativePath);

                if (!Directory.Exists(Path.GetDirectoryName(destinationFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
                }

                File.Copy(file, destinationFile, true);

                int encryptionTime = CryptoService.EncryptFile(destinationFile);
                Logger.Services.Logger.Instance.LogFileTransfert(new Logger.Models.LoggerModel
                {
                    TimeStamp = startTime,
                    SaveName = job.Name,
                    FileSize = new FileInfo(destinationFile).Length,
                    FileTransferTime = (DateTime.Now - startTime).TotalMilliseconds,
                    FileSource = file,
                    FileTarget = destinationFile,
                    EncryptionTime = encryptionTime
                });

                OnMessageLogged?.Invoke($"✅ Copied: {file} → {destinationFile}");
            }
            catch (Exception ex)
            {
                OnMessageLogged?.Invoke($"❌ Error copying {file}: {ex.Message}");
            }
            finally
            {
                if (fileSize > largeFileSizeLimit)
                {
                    largeFileSemaphore.Release();
                }
            }
        }
    }
}
