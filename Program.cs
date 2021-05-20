using System;
using System.IO;
namespace FileSystemWatcherPOC
{
    class Program
    {
        private static FileSystemWatcher _fileSystemWatcher;
        static void Main(string[] args)
        {
            try
            {
                var _fileSystemWatcher = new FileSystemWatcher(System.Environment.CurrentDirectory)
                {
                    Filter = "*.txt",
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size,
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true
                };
                _fileSystemWatcher.Changed += Notify;
                _fileSystemWatcher.Created += Notify;
                _fileSystemWatcher.Deleted += Notify;



                Console.WriteLine("press enter key to exit");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
           
        }

        private static void Notify(object sender,FileSystemEventArgs e)
        {
            Console.WriteLine($"{e.FullPath}-{e.ChangeType}");
        }
    }
}
