using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EasySave.Core.Models;
using EasySave.Core.Services;
using EasySave.RemoteConsole.Services;
using EasySave.RemoteConsole.Utils;

namespace EasySave.RemoteConsole.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TcpClientService _tcpClient;
        private string _consoleLog = "";
        private Job _selectedJob;

        public ObservableCollection<Job> Jobs { get; } = new(); // Job list
        public ObservableCollection<string> Logs { get; } = new(); // Console logs

        public string ConsoleLog
        {
            get => _consoleLog;
            set { _consoleLog = value; OnPropertyChanged(); }
        }

        public Job SelectedJob
        {
            get => _selectedJob;
            set
            {
                _selectedJob = value;
                OnPropertyChanged();
                (PauseCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (ResumeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (StopCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand PauseCommand { get; }
        public ICommand ResumeCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel()
        {
            _tcpClient = new TcpClientService();
            _tcpClient.Connect("127.0.0.1", 5000);

            PauseCommand = new RelayCommand(_ => SendCommand("pause"), _ => SelectedJob != null);
            ResumeCommand = new RelayCommand(_ => SendCommand("resume"), _ => SelectedJob != null);
            StopCommand = new RelayCommand(_ => SendCommand("stop"), _ => SelectedJob != null);

            // Listen for updates from the server
            _tcpClient.LogReceived += AppendConsoleLog;
            _tcpClient.JobsReceived += UpdateJobsList;

            // Request the current job list
            _tcpClient.SendCommand("list", "");
        }

        /// <summary>
        /// Updates the job list when new data is received from the server.
        /// </summary>
        private void UpdateJobsList(List<string> jobs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Jobs.Clear();

                foreach (var jobData in jobs)
                {
                    var jobEntries = jobData.Split('|'); // Sépare chaque job

                    foreach (var jobEntry in jobEntries)
                    {
                        string[] parts = jobEntry.Split(',');
                        AppendConsoleLog(parts.Length.ToString());

                        if (parts.Length >= 4)
                        {
                            if (!Enum.TryParse<SaveTypes>(parts[3], out var saveType))
                            {
                                saveType = SaveTypes.Complete;
                                AppendConsoleLog($"⚠ Erreur de conversion de SaveType pour le job: {parts[0]}, valeur reçue: {parts[3]}");
                            }

                            Jobs.Add(new Job
                            {
                                Name = parts[0],
                                SourcePath = parts[1],
                                DestinationPath = parts[2],
                                SaveType = saveType
                            });
                        }
                        else
                        {
                            AppendConsoleLog($"⚠ Erreur de parsing du job : {jobData} (Données insuffisantes)");
                        }
                    }
                }
            });
        }



        /// <summary>
        /// Sends a command to the server to control the selected job.
        /// </summary>
        private void SendCommand(string action)
        {
            if (SelectedJob != null)
            {
                _tcpClient.SendCommand(action, SelectedJob.Name);
                AppendConsoleLog(LanguageManager.GetString("CommandSent", action.ToUpper(), SelectedJob.Name));
            }
        }

        /// <summary>
        /// Appends a message to the console log.
        /// </summary>
        private void AppendConsoleLog(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Logs.Count > 0 && Logs[^1] == message) return; // Prevent duplicate logs
                Logs.Add(message);
                ConsoleLog += message + "\n";
                OnPropertyChanged(nameof(ConsoleLog));
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
