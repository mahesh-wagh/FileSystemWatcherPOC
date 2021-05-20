using System;
using System.IO;
namespace FileSystemWatcherPOC
{
    class Program
    {
        private static FileSystemWatcher _fileSystemWatcher;
        static void Main(string[] args)
        {

            _fileSystemWatcher = new FileSystemWatcher("Watcher", "*.*");

            _fileSystemWatcher.Changed +=Notify;
            _fileSystemWatcher.Created += Notify;
            _fileSystemWatcher.Deleted += Notify;
            _fileSystemWatcher.Renamed += Notify;
            _fileSystemWatcher.IncludeSubdirectories = true;
            _fileSystemWatcher.EnableRaisingEvents = true;

            Console.ReadKey(false);
        }

        private static void Notify(object sender,FileSystemEventArgs e)
        {
            Console.WriteLine($"{e.FullPath}-{e.ChangeType}");
        }
    }
}
