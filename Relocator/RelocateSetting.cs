using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relocator
{
    public class RelocateSetting
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public List<RelocateFile> Files { get; set; }       //this list holds several RelocateFiles so tha we can cater for several extensions

        public bool CanOverride { get; set; }
 
    }

   public  class RelocateFile
    {
        public string  Extention { get; set; }

        public List<System.IO.FileInfo> Files { get; set; }      
       
    }
}
