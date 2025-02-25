using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using Logger.Models;

namespace Logger.Services
{
    public class Logger
    {
        private static Logger instance;
        public static Logger Instance => instance ??= new Logger();

        private readonly string logDirectory;

        // 🔹 Constructeur : Initialise le dossier de logs
        private Logger()
        {
            ConfigManager config = new ConfigManager();
            config.LoadConfig();
            logDirectory = config.GetLogFilePath();

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory); // 🔹 Crée le dossier s'il n'existe pas
            }
        }

        /// <summary>
        /// Enregistre un transfert de fichier dans les logs.
        /// </summary>
        public void LogFileTransfert(LoggerModel loggerModel)
        {
            ConfigManager config = new ConfigManager();
            config.LoadConfig();
            string logFormat = config.GetLogFormat().ToLower();

            // 🔹 Vérification du format supporté
            if (logFormat != "json" && logFormat != "xml")
            {
                throw new ArgumentException("Format de log non supporté. Utilisez 'json' ou 'xml'.");
            }

            SaveLog(loggerModel, logFormat);
        }

        /// <summary>
        /// Sérialise un log en JSON ou XML et l'enregistre.
        /// </summary>
        private void SaveLog(LoggerModel log, string format)
        {
            string logFilePath = GetLogFilePath(format);
            List<LoggerModel> logs = LoadExistingLogs(logFilePath, format);

            logs.Add(log); // 🔹 Ajoute le nouveau log à la liste

            // 🔹 Écriture du fichier JSON ou XML
            if (format == "json")
            {
                string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(logFilePath, json);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, false)) // 🔹 `false` pour réécrire sans dupliquer
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<LoggerModel>));
                    serializer.Serialize(writer, logs);
                }
            }
        }

        /// <summary>
        /// Récupère le chemin du fichier log en fonction du format (JSON/XML).
        /// </summary>
        private string GetLogFilePath(string format)
        {
            string extension = format == "xml" ? "xml" : "json";
            string fileName = $"Log-{DateTime.Now:yyyy-MM-dd}.{extension}";
            return Path.Combine(logDirectory, fileName);
        }

        /// <summary>
        /// Charge les logs existants pour éviter d'écraser les anciens.
        /// </summary>
        private List<LoggerModel> LoadExistingLogs(string filePath, string format)
        {
            if (!File.Exists(filePath))
                return new List<LoggerModel>();

            try
            {
                if (format == "json")
                {
                    string json = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<List<LoggerModel>>(json) ?? new List<LoggerModel>();
                }
                else
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<LoggerModel>));
                        return (List<LoggerModel>)serializer.Deserialize(reader) ?? new List<LoggerModel>();
                    }
                }
            }
            catch
            {
                return new List<LoggerModel>(); // 🔹 En cas d'erreur, retourne une liste vide
            }
        }
    }
}
