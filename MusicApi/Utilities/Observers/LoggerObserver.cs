using MusicApi.Abstract;
using System;
using System.IO;

namespace MusicApi.Utilities.Observers
{
    public class LoggerObserver : IObserver
    {
        private readonly string _logFilePath;

        public LoggerObserver()
        {
            _logFilePath = "logs.txt";
        }

        public void Update(string message)
        {
            try
            {
                // Verifica se il file esiste, altrimenti crealo
                if (!File.Exists(_logFilePath))
                {
                    // Crea il file e chiudilo immediatamente
                    File.Create(_logFilePath).Dispose();
                }

                // Scrivi il messaggio nel file di log
                File.AppendAllText(_logFilePath, message + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to log file: " + ex.Message);
            }
        }
    }
}
