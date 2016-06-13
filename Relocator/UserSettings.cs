using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Relocator
{
    //This is a singleton class, meaning only one instance can be created.
    //This class saves user preferences into an Xml file in the directory of the program.
    class UserSettings
    {
        public static List<Relocators> relocatorPrefs { get; set; }

        private UserSettings instance;

        //Class serializer
        private static XmlSerializer serializer;

        //preferences destination path
        private const String PREFERENCE_PATH = "preference.xml"; 

        public UserSettings Instance
        {
            get
            {
               if(instance == null)
                {
                    instance = new UserSettings();
                }
                return instance;
            }
        }

        private UserSettings()
        {
            //Initialize the class
            init();
        }    

        private void init()
        {
            //Load the preferences from a file if existent... else wait till the user adds some prefs
            relocatorPrefs = new List<Relocators>();
            serializer = new XmlSerializer(typeof(List<RelocateFile>));

            relocatorPrefs = Deserialize();         //deserialize the data
        }

        //This function is to update our saved preferences
        //Simply adds a Relocator to our preferences file
        public void addPreference(Relocators relocator)
        {
            relocatorPrefs.Add(relocator);

            //Now we serialize the whole data again
    
            using (TextWriter writer = new StreamWriter(@PREFERENCE_PATH))
            {
                serializer.Serialize(writer, relocatorPrefs);
            }
        }
         
        //Gets object from serialized data
        public List<Relocators> Deserialize()
        {
            List<Relocators> relocators = new List<Relocators>();
            try
            {
                using (TextReader reader = new StreamReader(@PREFERENCE_PATH))
                {
                    Object obj = serializer.Deserialize(reader);
                    relocators = (List<Relocators>)obj;
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Can't find preference file... would create one");
            }
                return relocators;
        }
    }
}
