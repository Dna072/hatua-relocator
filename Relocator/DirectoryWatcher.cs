using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace Relocator
{
    class DirectoryWatcher
    {
        private static List<Relocators> relocators;
        private static List<FileSystemWatcher> fileWatcher;
        
        //This is a singleton class
        private static DirectoryWatcher instance;

        private DirectoryWatcher()
        {
            relocators = new List<Relocators>();
            fileWatcher = new List<FileSystemWatcher>();
        }

        public static DirectoryWatcher Instance
        {
            get {
                if (instance == null) {
                    instance = new DirectoryWatcher();
                }
                return instance;
            }
        }



        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        //Initialize directory watcher with the extensions it should look out for...
        //for each "Relocator"
        public void init()      
        {
            Parallel.ForEach(relocators, (relocator) =>
            {
                List<string> extensions = relocator.getExtensions();

                foreach (string ext in extensions) {
                    FileSystemWatcher watcher = new FileSystemWatcher(relocator.RelocateSetting.Source);
                    watcher.Filter = String.Format("*{0}", ext);

                    watcher.Created += new FileSystemEventHandler(relocator.moveFile);

                    //Begin monitoring
                    watcher.EnableRaisingEvents = true;
                }
            });
        }

        public void Add(Relocators relocator)
        {
            relocators.Add(relocator);
        }

    }
}
