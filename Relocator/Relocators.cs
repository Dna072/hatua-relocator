using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relocator
{
    public class Relocators
    {
        public RelocateSetting RelocateSetting { get; set; }
        private List<string> fileExtensions;

        
        public Relocators()
        {

        }

        public Relocators(RelocateSetting relocateSetting)
            : this()
        {
            this.RelocateSetting = relocateSetting;
        }

        public bool DirectoryExist()
        {
            if (Directory.Exists(RelocateSetting.Source))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SetFileExtensions(string userExtensionRequest)
        {
            //Split File Extension 
            fileExtensions = new List<string>(userExtensionRequest.Split(','));
        }

        public List<RelocateFile> GetFiles()
        {
            
            DirectoryInfo di = new DirectoryInfo(@RelocateSetting.Source);
            List<RelocateFile> relocateFiles = new List<RelocateFile>(); 

            foreach (var ext in fileExtensions)
            {
                List<FileInfo> files = new List<FileInfo>(di.GetFiles(String.Format("*{0}", ext)));
                
                if (files.Count != 0)
                {
                                           
                     relocateFiles.Add(new RelocateFile() { Extention = ext, Files = files });                       
                     
                }
            }

            return relocateFiles;
        }
                        

        internal void ProcessJob(List<RelocateFile> list)
        {
            //For each RelocateFile, check for duplicates
            //If overwrite option is selected, delete destination duplicates
            //else do not move those files

            foreach (RelocateFile file in list) {
                List<string> duplicateFiles = filesAlreadyInDestination(file, RelocateSetting.Source);

                if (duplicateFiles.ToArray().Length > 0)
                {
                    //Check the value of override,
                    if(RelocateSetting.CanOverride)
                    {
                        //if true: delete copies in dest
                        //move
                        deleteFiles(duplicateFiles, file);
                        moveFiles(file);
                    }
                    else
                    {
                        //perform move method on original files
                        moveFiles(file);
                    }
                }else
                {
                    moveFiles(file);
                }                
                
            }            
        }

        public List<string> filesAlreadyInDestination(RelocateFile files, string source)
        {
            List<string> duplicateFiles = new List<string>();

            foreach (FileInfo file in files.Files) {
                string destinationPath = RelocateSetting.Destination + "\\" + file.Name;

                if (File.Exists(destinationPath)) {
                    duplicateFiles.Add(file.Name);
                }
            }
                    
               
            return duplicateFiles;
        }

        //@param fileNames Holds the file names that are to be deleted
        //@param files Holds the RelocateFile that contains the details of all files that are to be moved
        public void deleteFiles(List<string> filesNames, RelocateFile files)
        {
            //compare the files and then delete the ones that match
            //delete in the destination!!
            foreach (string name in filesNames) {
                foreach (FileInfo fileInfo in files.Files) {
                    if (name.Equals(fileInfo.Name)) {
                        string destinationPath = RelocateSetting.Destination + "\\" + fileInfo.Name;
                        File.Delete(destinationPath);
                    }
                }
            }
        }

        public void moveFiles(RelocateFile file)
        {
            if (RelocateSetting.CanOverride)
            {
                /*foreach (FileInfo fileInfo in file.Files)
                {
                    string destinationPath = file.Destination + "\\" + fileInfo.Name;
                    fileInfo.MoveTo(destinationPath);
                }*/

                Parallel.ForEach(file.Files, (movingFile) =>
                {
                    string destinationPath = RelocateSetting.Destination + "\\" + movingFile.Name;
                    movingFile.MoveTo(destinationPath);
                });
            }
            else {
                /*foreach (FileInfo fileInfo in file.Files)
                {
                    string destinationPath = RelocateSetting.Destination + "\\" + fileInfo.Name;

                    try
                    {
                        fileInfo.MoveTo(destinationPath);
                    }
                    catch (IOException)
                    {
                        //do nothing
                       
                    }
                }*/

                Parallel.ForEach(file.Files, (fileInfo)=>{
                    string destinationPath = RelocateSetting.Destination + "\\" + fileInfo.Name;

                    try
                    {
                        fileInfo.MoveTo(destinationPath);
                    }
                    catch (IOException)
                    {
                        //do nothing

                    }
                });
            }

        }

        public List<string> getExtensions()
        {
            return fileExtensions;
        }

        //Event handler for moving files
        public void moveFile(object source, FileSystemEventArgs e) {
            //Moving file from source
            string destinationPath = RelocateSetting.Destination + "\\" + e.Name;

            if (File.Exists(destinationPath))
            {
                if (RelocateSetting.CanOverride)
                {
                    //delete the file at destination
                    File.Delete(destinationPath);
                    File.Move(e.FullPath, destinationPath);
                    Console.WriteLine("File: " + e.Name + " moved to " + RelocateSetting.Destination);
                }
                else
                {
                    Console.WriteLine("File: " + e.Name + "not moved to due to duplicate");
                }
            }
            else {
                File.Move(e.FullPath, destinationPath);
                Console.WriteLine("File: " + e.Name + " moved to " + RelocateSetting.Destination);
            }

        }
    }
}
