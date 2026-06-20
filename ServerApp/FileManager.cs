using System;
using System.IO;

namespace ServerApp
{
    public static class FileManager
    {
        private static string _storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReceivedFiles");

        public static string GetStoragePath()
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            return _storagePath;
        }
    }
}