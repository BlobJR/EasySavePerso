using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using EasySave.Core.Services;

namespace EasySave.RemoteConsole.Services
{
    public class TcpClientService
    {
        // TCP client for connecting to the server
        private TcpClient _client;

        // Network stream for reading and writing data
        private NetworkStream _stream;

        // Event triggered when the list of jobs is received from the server
        public event Action<List<string>>? JobsReceived;

        // Event triggered when a log message is received from the server
        public event Action<string>? LogReceived;

        /// <summary>
        /// Attempts to connect to the server with a retry mechanism.
        /// </summary>
        /// <param name="serverIp">The IP address of the server.</param>
        /// <param name="port">The port to connect to.</param>
        public void Connect(string serverIp, int port)
        {
            int maxAttempts = 10; // Maximum number of connection attempts
            int attempt = 0;

            while (attempt < maxAttempts)
            {
                try
                {
                    // Initialize a new TCP client and attempt to connect
                    _client = new TcpClient();
                    _client.Connect(serverIp, port);
                    _stream = _client.GetStream();
                    StartListening();

                    // Notify the UI that the connection was successful
                    Application.Current.Dispatcher.Invoke(() =>
                        LogReceived?.Invoke(LanguageManager.GetString("ServerConnected", serverIp, port))
                    );
                    return; // Exit loop if connection is successful
                }
                catch (SocketException)
                {
                    attempt++;

                    // Notify UI about the connection attempt failure
                    Application.Current.Dispatcher.Invoke(() =>
                        LogReceived?.Invoke(LanguageManager.GetString("ServerNotReady", attempt, maxAttempts))
                    );

                    Thread.Sleep(3000); // Wait for 3 seconds before retrying
                }
            }

            // Notify UI if the connection fails after maximum attempts
            Application.Current.Dispatcher.Invoke(() =>
                LogReceived?.Invoke(LanguageManager.GetString("ServerConnectionFailed"))
            );
        }

        /// <summary>
        /// Starts listening for incoming messages from the server.
        /// </summary>
        private void StartListening()
        {
            Thread listenerThread = new Thread(() =>
            {
                byte[] buffer = new byte[1024];

                try
                {
                    while (true)
                    {
                        int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead <= 0) break; // Exit loop if no data is received

                        // Convert received bytes into a string message
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                        // Process the received message
                        Application.Current.Dispatcher.Invoke(() => ProcessReceivedMessage(message));
                    }
                }
                catch { }
            });

            listenerThread.IsBackground = true;
            listenerThread.Start(); // Start the listener thread
        }

        /// <summary>
        /// Processes messages received from the server.
        /// </summary>
        /// <param name="message">The received message string.</param>
        private void ProcessReceivedMessage(string message)
        {
            // Check if the message is a log update
            if (message.StartsWith("TYPE=LOG"))
            {
                string logContent = message.Split("MESSAGE=")[1]; // Extract log content
                LogReceived?.Invoke(logContent); // Notify listeners (e.g., UI)
            }
            // Check if the message contains a list of jobs
            else if (message.StartsWith("TYPE=JOBS"))
            {
                string jobList = message.Split("LIST=")[1]; // Extract job list
                List<string> jobs = jobList.Split('|').ToList(); // Convert to list
                JobsReceived?.Invoke(jobs); // Notify listeners
            }
        }

        /// <summary>
        /// Sends a command to the server to execute an action on a job.
        /// </summary>
        /// <param name="action">The action to perform (e.g., "PAUSE", "RESUME", "STOP").</param>
        /// <param name="jobName">The name of the job to execute the action on.</param>
        public void SendCommand(string action, string jobName)
        {
            // Ensure that the client is connected before sending a command
            if (_client == null || !_client.Connected)
            {
                Application.Current.Dispatcher.Invoke(() =>
                    LogReceived?.Invoke(LanguageManager.GetString("ServerNotConnected"))
                );
                return;
            }

            // Format the command string
            string command = $"TYPE=COMMAND;ACTION={action.ToUpper()};JOB={jobName}";

            try
            {
                // Convert the command to bytes and send it to the server
                byte[] data = Encoding.UTF8.GetBytes(command);
                _stream.Write(data, 0, data.Length);
                _stream.Flush(); // Ensure the data is sent immediately

                // Notify UI about the command being sent
                Application.Current.Dispatcher.Invoke(() =>
                    LogReceived?.Invoke(LanguageManager.GetString("CommandSent", action.ToUpper(), jobName))
                );
            }
            catch (Exception ex)
            {
                // Notify UI if an error occurs while sending the command
                Application.Current.Dispatcher.Invoke(() =>
                    LogReceived?.Invoke(LanguageManager.GetString("CommandFailed", action.ToUpper(), jobName, ex.Message))
                );
            }
        }
    }
}
