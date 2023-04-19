using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using task2np.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace task2np
{
    public class ContainerCollection
    {
        private List<Container> collection;
        private HashSet<string> ids;
        public ContainerCollection(params Container[] conts)
        {
            collection = new List<Container>();
            ids = new HashSet<string>();
            foreach (var cont in conts)
            {
                Add(cont);
            }

        }
        public void Add(Container cont)
        {
            if(ContainerValidation.NewIdValidation(this, cont.Id))
            {
                collection.Add(cont);
                ids.Add(cont.Id);
            }
            else
            {
                Console.WriteLine("element was not added!");
            }
        }
        public override string ToString()
        {
            string res = "";
            foreach (var cont in collection)
            {
                res += cont.ToString() + "\n";
            }
            return res;
        }
        public HashSet<string> GetIds() { return ids;}
        public void DeleteById(string id)
        {
            if(ids.Contains(id))
            {
                foreach(var cont in collection)
                {
                    if(cont.Id == id)
                    {
                        ids.Remove(id);
                        collection.Remove(cont);
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentException("element was not deleted, element with that id didn't exist!");
            }
        }
        public void EditById(string id, string prop, string new_value)
        {
            if (ids.Contains(id))
            {
                foreach (var cont in collection)
                {
                    if (cont.Id == id)
                    {
                        if(prop == "id" && new_value != id)
                        {
                            if(!ContainerValidation.NewIdValidation(this, new_value))
                            {
                                throw new ArgumentException("element with this id already exists");
                            }
                            ids.Remove(id);
                            ids.Add(new_value);
                            typeof(Container).GetProperty(prop.ToPascalCase()).SetValue(this, Convert.ChangeType(new_value, GetType().GetProperty(prop.ToPascalCase()).PropertyType));
                            return;
                        }
                        if (typeof(Container).GetProperty(prop.ToPascalCase()).PropertyType.IsEnum)
                        {
                            typeof(Container).GetProperty(prop.ToPascalCase()).SetValue(cont, new_value.ParseCity());
                        }
                        else
                        {
                            typeof(Container).GetProperty(prop.ToPascalCase()).SetValue(cont, Convert.ChangeType(new_value, typeof(Container).GetProperty(prop.ToPascalCase()).PropertyType));
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("element was not edited, element with that id doesn't exist!");
            }
        }

        public void ReadJsonFile(string path)
        {
            if(SideValidation.FileNameValidation(path, "json") && SideValidation.FileExist(path))
            {
                
                string errors = "";
                collection = new List<Container>();
                ids = new HashSet<string>();
                string text = SideValidation.ReadFile(path);
                List<Dictionary<string, object>> c = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(text);
                foreach (var j in c)
                {
                    try
                    {
                        collection.Add(new Container(j));
                        ids.Add(collection[collection.Count - 1].Id);
                    }
                    catch(ArgumentException e) {
                        errors += "Errors in element!!:\n" + e.Message + "\n";
                    }
                }
                if(errors.Length > 0)
                {
                    throw new ArgumentException(errors);
                }
                return;
            }
            throw new ArgumentException("filename is invalid");
        }

        public void WriteJsonFile(string path)
        {
            if (SideValidation.FileNameValidation(path, "json"))
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                SideValidation.WriteFile(path, System.Text.Json.JsonSerializer.Serialize(collection, options));
                return;

            }
            throw new ArgumentException("filename is invalid");
        }

        public ContainerCollection Search(string search)
        {
            ContainerCollection found = new ContainerCollection();
            foreach(var x in collection)
            {
                foreach (var p in typeof(Container).GetProperties())
                {
                    if (p.GetValue(x).ToString().ToLower().Contains(search.ToLower()))
                    {
                        found.Add(x);
                        break;
                    }
                }
            }
            return found;
        }
        public void Sort(string sorting_attr)
        {
            var snakeCaseStrategy = new SnakeCaseNamingStrategy();
            sorting_attr = sorting_attr.ToPascalCase();
            if (!typeof(Container).GetProperties().Any(x => x.Name == sorting_attr))
            {
                sorting_attr = typeof(Container).GetProperties()[0].Name;
            }

            if (typeof(Container).GetProperty(sorting_attr).GetType().IsEnum)
            {
                collection.Sort((a, b) => (Enum.GetName(typeof(City), (City)typeof(Container).GetProperty(sorting_attr).GetValue(b)).CompareTo(Enum.GetName(typeof(City), (City)typeof(Container).GetProperty(sorting_attr).GetValue(b)))));
            }
            else
            {
                collection = collection.OrderBy(a => typeof(Container).GetProperty(sorting_attr).GetValue(a)).ToList();
            }
        }

    }
    
}
