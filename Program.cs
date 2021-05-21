using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace FileSystemWatcherPOC
{
    class Program
    {
        private static FileSystemWatcher _fileSystemWatcher;

        private static IChangeToken _fileChangeToken;
        private static PhysicalFileProvider _fileProvider;
        private static readonly ConcurrentDictionary<string, DateTime> _files = new ConcurrentDictionary<string, DateTime>();
        private static string _filePath= "/app/another";
        public static void DoWork()
        {
            Console.WriteLine("Using PhysicalFileWatcher");
            _fileProvider = new PhysicalFileProvider(_filePath); // e.g. C:\temp
            WatchForFileChanges();
        }

        private static void WatchForFileChanges()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(_filePath, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (_files.TryGetValue(file, out DateTime existingTime))
                {
                    _files.TryUpdate(file, File.GetLastWriteTime(file), existingTime);
                }
                else
                {
                    if (File.Exists(file))
                    {
                        _files.TryAdd(file, File.GetLastWriteTime(file));
                    }
                }
            }
            _fileChangeToken = _fileProvider.Watch("**/*.*");
            _fileChangeToken.RegisterChangeCallback(Notify, default);
        }
        static void Main(string[] args)
        {
            try
            {

                DoWork();




                //var filePath = "/app/another";
                //Console.WriteLine(System.Environment.CurrentDirectory);
                //var _fileSystemWatcher = new FileSystemWatcher(filePath)
                //{
                //    Filter = "*.txt",
                //    NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size,
                //    EnableRaisingEvents = true,
                //    IncludeSubdirectories = true
                //};

                //_fileSystemWatcher.Changed += Notify;
                //_fileSystemWatcher.Created += Notify;
                //_fileSystemWatcher.Deleted += Notify;



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

        private static void Notify(object state)
        {
            Console.WriteLine("File activity detected.");
            WatchForFileChanges();
        }
    }
}
