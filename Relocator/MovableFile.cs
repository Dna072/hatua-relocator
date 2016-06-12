using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Relocator
{
    class MovableFile
    {
        public String extension
        {
            get;
            set;
        }
        public int count
        {
            get;
            set;
        }
      public String   destination { get; set; }
        public String source { get; set; }

        public MovableFile(String ext, int count, string dest, string source)
        {
            extension = ext;
            this.count = count;
            destination = dest;
            this.source = source;
        }

       public void moveToDirectory(List<string> fileNames)
        {
            List<string> finalFilesToBeMoved = new List<string>();
            if (count > 0)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                /*List<Task> holdTasks  = new List<Task>();  
                foreach (string x in fileNames) {
                      
                      var t1 = Task.Factory.StartNew(() =>
                      {
                          Thread.Sleep(1000);
                          string fullDestPath = destination + '\\' + Path.GetFileName(x);
                          Console.WriteLine("moving file: " + Path.GetFileName(x));
                          File.Move(x, fullDestPath);
                      });
                      holdTasks.Add(t1);
                      
                  }
                   Task.WaitAll(holdTasks.ToArray());
                  
                //Get time elasped
                  Console.WriteLine("Time elapsed(ms): " + sw.ElapsedMilliseconds);
             */
                bool yesAlwaysPerform = false;
                bool overwrite = false;
       
                foreach(string file in fileNames){
                    string fullDestPath = destination + '\\' + Path.GetFileName(file);

                    if (File.Exists(fullDestPath))
                    {

                        
                        if (yesAlwaysPerform) {
                            alwaysPerform(finalFilesToBeMoved, fullDestPath, file, overwrite );
                        }

                        else
                        {
                            doNotAlwaysPerform(finalFilesToBeMoved, fullDestPath, file, ref overwrite);
                            Console.Write("Always perform this action?: ");
                            
                              String yas= Console.ReadLine();
                            if(yas.Equals("yes"))
                            {
                                yesAlwaysPerform=true;
                            }
                        }
                         
                       
                                         }
                    else {
                        finalFilesToBeMoved.Add(file);
                    }
                }

                Parallel.ForEach(finalFilesToBeMoved, (currentFile) =>
                {
                    string fullDestPath = destination + '\\' + Path.GetFileName(currentFile);

                    try
                    {

                        Thread.Sleep(1000);
                        Console.WriteLine("moving file: " + Path.GetFileName(currentFile));
                        File.Move(currentFile, fullDestPath);
                        

                    }

                    catch (UnauthorizedAccessException e)
                    {
                        Console.WriteLine(e.Message);

                        Console.WriteLine("Consider running in administartor mode");
                    }

                    catch (Exception x)
                    {
                        Console.WriteLine(x.Message);
                        /*if (File.Exists(fullDestPath))
                        {
                            {

                                File.Move(currentFile, fullDestPath);
                            }
                        }*/

                    }
                  
                                });
                  sw.Stop();
                  Console.WriteLine(finalFilesToBeMoved.ToArray().Length + " files moved");
                 // Get time elasped
                  Console.WriteLine("Time elapsed(ms): " + sw.ElapsedMilliseconds);
 
            }
            
        }

      public List<string> doNotAlwaysPerform(List<string> finalFilesToBeMoved, string fullDestPath, string file, ref bool overwrite)
       {
           Console.WriteLine(Path.GetFileName(file) + " already exists in destination folder");
           Console.Write("do you want to overwrite? yes or no:  ");
           string confirm = Console.ReadLine();



           if (confirm.Equals("yes"))
           {
               overwrite = true;
               File.Delete(fullDestPath);
               finalFilesToBeMoved.Add(file);
           }
           return finalFilesToBeMoved;
       }


    public List<string> alwaysPerform(List<string> finalFilesToBeMoved, string fullDestPath, string file, bool overwrite)
      {
          if (overwrite)
          {
              File.Delete(fullDestPath);
              finalFilesToBeMoved.Add(file);
          }
          

          return finalFilesToBeMoved;

      }
             
 

    }
}
