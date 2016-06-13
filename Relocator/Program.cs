using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Relocator
{
    class Program
    {
        private static RelocateSetting relocateSetting;
        private static Relocators relocators;

        //The directory monitor that monitors all source directories
        private static DirectoryWatcher directoryWatcher = DirectoryWatcher.Instance;

        static void Main(string[] args)
        {
            var sentinel = "Y";
            string userTerminateResponse = "no";

            relocateSetting = new RelocateSetting();
            relocators = new Relocators();

            while (!(sentinel.Equals("no")))
            {
                relocateSetting = new RelocateSetting();
                relocators = new Relocators();
                try
                {
                    Console.Write("Enter source directory: ");
                    //sourcePath = Console.ReadLine();
                    relocateSetting.Source = Console.ReadLine();
                    relocators.RelocateSetting = relocateSetting;

                    if (!relocators.DirectoryExist())
                    {
                        Console.WriteLine("Directory doesn't exist");
                    }
                    else
                    {
                        Console.Write("Enter file extensions(eg: .pdf), separate multiple with comma, no spaces: ");
                        string userExtensionRequest = Console.ReadLine();
                        relocators.SetFileExtensions(userExtensionRequest);
                        relocateSetting.Files = relocators.GetFiles();

                        if (relocateSetting.Files.Count !=0 )
                        {
                            // foreach (var ext in relocateSetting.Files)
                            //  {
                            //Console.Write("Enter destination for " + ext.Extention + ": ");
                            Console.Write("Enter destination for files: ");
                            relocateSetting.Destination = Console.ReadLine();
                                //relocateSetting.Destination = ext.Destination;
                           // }

                            Console.WriteLine("Overwrite file if there's a copy? yes or no");
                            String overRideDecision = Console.ReadLine().ToLower();
                            {
                                if(overRideDecision.Equals("yes"))
                                {
                                    relocateSetting.CanOverride = true;
                                }

                                else
                                {
                                    relocateSetting.CanOverride = false;
                                }
                            }
                            relocators.ProcessJob(relocateSetting.Files);

                            //Now begin to watch the directory
                            directoryWatcher.Add(relocators);
                            directoryWatcher.init();
                        }

                        else
                        {
                            Console.WriteLine("no such extension found");
                        }
                        
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Do you want to perform another operation?");
                userTerminateResponse = Console.ReadLine();
                sentinel = userTerminateResponse.ToLower();
            }
            Console.WriteLine("Enter any key");
            Console.ReadKey();
        }

        private static void ProcessRelocation(RelocateSetting relocateSetting)
        {
            relocators = new Relocators(relocateSetting);

            //Check If Directory Exist
            //Get the list of the files in the directory
        }

       
    }
}
