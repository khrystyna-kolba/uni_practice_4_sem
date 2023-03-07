using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace np_4sem_proj
{
    internal class SideValidation
    {
        public static bool FileNameValidation(string name, string type)
        { 
            Regex re = new Regex($"^[^</*?\"\\\\>:|]+.{type}$");
            return re.IsMatch(name);
        }
    }
}
