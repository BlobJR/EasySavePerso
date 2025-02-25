using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using EasySave.Core.Models;

namespace EasySave.Core.Services
{
    public class RemoteConsoleServer
    {
        public event Action<string>? OnMessageLogged;
        // Socket for listening to client connections
        private Socket _listener;

        // Dictionary to store connected clients (name -> socket)
        private Dictionary<string, Socket> _clients = new();

        // SaveManager instance to handle job operations
        private readonly SaveManager _saveManager = new();

        // Singleton instance of RemoteConsoleServer
        public static RemoteConsoleServer Instance { get; } = new RemoteConsoleServer();

        // Private constructor for Singleton pattern
        private RemoteConsoleServer() { }

        /// <summary>
        /// Starts the server and listens for incoming client connections.
        /// </summary>
        /// <param name="port">The port number to bind the server to.</param>
        public void StartServer(int port = 5000)
        {
            if (_listener != null)
            {
                LogMessage(LanguageManager.GetString("ServerAlreadyRunning"));
                return;
            }
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, port));
            _listener.Listen(16);
            SendJobsListToAllClients();
            LogMessage(LanguageManager.GetString("ServerStarted", port));

            while (true)
            {
                try
                {
                    // Accept a new client connection
                    Socket client = _listener.Accept();
                    string name = ReadName(client);
                    if (string.IsNullOrEmpty(name)) continue;

                    // Store client socket
                    _clients[name] = client;
                    LogMessage(LanguageManager.GetString("ClientConnected", name));

                    // Start handling client communication on a new thread
                    new Thread(() => HandleClient(client, name)).Start();
                }
                catch { break; } // If an exception occurs, break the loop
            }

            // Cleanup: Dispose all client sockets when the server stops
            foreach (var client in _clients.Values)
                client.Dispose();
        }

        /// <summary>
        /// Reads the client's name when they connect.
        /// </summary>
        /// <param name="client">The socket representing the client.</param>
        /// <returns>The client's name, or an empty string if invalid.</returns>
        private string ReadName(Socket client)
        {
            byte[] data = new byte[1024];
            int count = client.Receive(data);
            string name = Encoding.UTF8.GetString(data, 0, count).Trim();

            // Check if the name is already used
            if (_clients.ContainsKey(name))
            {
                SendToClient(client, LanguageManager.GetString("ClientNameError"));
                client.Dispose();
                return "";
            }

            // Send welcome message to the client
            SendToClient(client, LanguageManager.GetString("ClientWelcome", name));
            return name;
        }

        /// <summary>
        /// Handles communication with a connected client.
        /// </summary>
        /// <param name="client">The socket representing the client.</param>
        /// <param name="name">The name of the client.</param>
        private void HandleClient(Socket client, string name)
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    // Receive data from the client
                    int count = client.Receive(buffer);
                    if (count <= 0) break;

                    string command = Encoding.UTF8.GetString(buffer, 0, count).Trim();
                    LogMessage(LanguageManager.GetString("CommandReceived", name, command));

                    // Process the received command
                    ProcessCommand(command, name);
                }
            }
            catch { }

            // Remove client when they disconnect
            _clients.Remove(name);
            client.Dispose();
            LogMessage(LanguageManager.GetString("ClientDisconnected", name));
        }

        /// <summary>
        /// Processes commands received from clients.
        /// </summary>
        /// <param name="command">The command string received from the client.</param>
        /// <param name="sender">The name of the client sending the command.</param>
        private void ProcessCommand(string command, string sender)
        {
            Dictionary<string, string> parsedCommand = ParseMessage(command);
            if (!parsedCommand.ContainsKey("TYPE") || parsedCommand["TYPE"] != "COMMAND") return;

            if (!parsedCommand.ContainsKey("ACTION") || !parsedCommand.ContainsKey("JOB"))
            {
                SendToClient(sender, LanguageManager.GetString("InvalidCommand"));
                return;
            }

            string action = parsedCommand["ACTION"].ToLower();
            string jobName = parsedCommand["JOB"];

            Job? job = JobManager.Instance.SearchJob(jobName);
            if (job == null)
            {
                SendToClient(sender, LanguageManager.GetString("JobNotFound", jobName));
                return;
            }

            switch (action)
            {
                case "pause":
                    //_saveManager.PauseJob(new List<Job> { job });
                    SendToClient(sender, LanguageManager.GetString("JobPaused", jobName));
                    LogMessage(LanguageManager.GetString("JobPausedLog", jobName));
                    break;
                case "resume":
                    //_saveManager.ResumeJob(new List<Job> { job });
                    SendToClient(sender, LanguageManager.GetString("JobResumed", jobName));
                    LogMessage(LanguageManager.GetString("JobResumedLog", jobName));
                    break;
                case "stop":
                    //_saveManager.StopJob(new List<Job> { job });
                    SendToClient(sender, LanguageManager.GetString("JobStopped", jobName));
                    LogMessage(LanguageManager.GetString("JobStoppedLog", jobName));
                    break;
                case "list":
                    SendJobsListToAllClients();
                    break;
                default:
                    SendToClient(sender, LanguageManager.GetString("UnknownAction", action));
                    break;
            }
        }

        /// <summary>
        /// Sends a message to a specific client by name.
        /// </summary>
        /// <param name="clientName">The name of the client.</param>
        /// <param name="message">The message to send.</param>
        private void SendToClient(string clientName, string message)
        {
            if (_clients.ContainsKey(clientName))
            {
                SendToClient(_clients[clientName], message);
            }
        }

        /// <summary>
        /// Sends a message to a specific client socket.
        /// </summary>
        /// <param name="client">The socket representing the client.</param>
        /// <param name="message">The message to send.</param>
        private void SendToClient(Socket client, string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes($"TYPE=RESPONSE;STATUS=OK;MESSAGE={message}");
                client.Send(data);
            }
            catch { }
        }

        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendToAll(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes($"TYPE=LOG;MESSAGE={message}");

            foreach (var client in _clients.Values)
            {
                try
                {
                    client.Send(data);
                }
                catch { }
            }
        }

        /// <summary>
        /// Parses a command string into key-value pairs.
        /// </summary>
        /// <param name="message">The command string.</param>
        /// <returns>A dictionary containing parsed key-value pairs.</returns>
        private Dictionary<string, string> ParseMessage(string message)
        {
            Dictionary<string, string> parsed = new();
            string[] parts = message.Split(';');

            foreach (var part in parts)
            {
                string[] kv = part.Split('=');
                if (kv.Length == 2)
                {
                    parsed[kv[0].Trim()] = kv[1].Trim();
                }
            }

            return parsed;
        }

        /// <summary>
        /// Logs a message to the console and sends it to all clients.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogMessage(string message)
        {
            OnMessageLogged?.Invoke(message);
            SendToAll(message);
        }

        /// <summary>
        /// Sends the list of jobs to all connected clients.
        /// </summary>
        public void SendJobsListToAllClients()
        {
            // Retrieve the job list from JobManager
            List<Job> jobs = JobManager.Instance.ListJobs(false);
            string jobListMessage = "TYPE=JOBS;LIST=" + string.Join("|", jobs.Select(j => $"{j.Name},{j.SourcePath},{j.DestinationPath},{j.SaveType}"));


            foreach (var client in _clients.Values)
            {
                try
                {
                    client.Send(Encoding.UTF8.GetBytes(jobListMessage));
                }
                catch { }
            }
        }
        public void StopServer()
        {
            try
            {
                LogMessage(LanguageManager.GetString("ServerStopping"));

                // Ferme toutes les connexions clients
                foreach (var client in _clients.Values)
                {
                    try
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                    catch { }
                }

                _clients.Clear();

                // Arrête le serveur s'il est actif
                if (_listener != null)
                {
                    _listener.Close();
                    _listener = null;
                }

                LogMessage(LanguageManager.GetString("ServerStopped"));
            }
            catch (Exception ex)
            {
                LogMessage(LanguageManager.GetString("ServerStopError", ex.Message));
            }
        }


    }
}
