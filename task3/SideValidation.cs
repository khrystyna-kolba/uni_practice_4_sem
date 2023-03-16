using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ContainerUniversalTest
{
    internal class SideValidation
    {
        public static bool FileNameValidation(string name, string type)
        { 
            Regex re = new Regex($"^[^</*?\"\\\\>:|]+.{type}$");
            return re.IsMatch(name);
        }
        public static bool FileExist(string path)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            //var jarray = new JArray();
            try
            {
                string text = (File.ReadAllText(projectDirectory + "\\" + path));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string ReadFile(string path)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            //var jarray = new JArray();
            try
            {
                string text = (File.ReadAllText(projectDirectory + "\\" + path));
                return text;
            }
            catch
            {
                throw new FileNotFoundException("file was not found");
            }
        }

        public static void WriteFile(string path, string info)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            File.WriteAllText(projectDirectory + "\\" + path, info);
        }
    }
}
