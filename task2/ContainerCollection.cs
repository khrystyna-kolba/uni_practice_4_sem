using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using np_4sem_proj.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace np_4sem_proj
{
    public class ContainerCollection
    {
        private List<Container> containers;
        private HashSet<string> ids;
        public ContainerCollection(params Container[] conts)
        {
            containers = new List<Container>();
            ids = new HashSet<string>();
            foreach (var cont in conts)
            {
                this.Add(cont);
            }

        }
        public void Add(Container cont)
        {
            if(ContainerValidation.NewIdValidation(this, cont.Id))
            {
                containers.Add(cont);
                ids.Add(cont.Id);
            }
            else
            {
                Console.WriteLine("container was not added!");
            }
        }
        public override string ToString()
        {
            string res = "";
            foreach (var cont in containers)
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
                foreach(var cont in containers)
                {
                    if(cont.Id == id)
                    {
                        ids.Remove(id);
                        containers.Remove(cont);
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentException("container was not deleted, container with that id didn't exist!");
            }
        }

        public void EditById(string id, string prop, string new_value)
        {
            if (ids.Contains(id))
            {
                foreach (var cont in containers)
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
                            return;
                        }
                        cont.SetProp(prop, new_value);
                    }
                }
            }
            else
            {
                throw new ArgumentException("container was not edited, container with that id doesn't exist!");
            }
        }

        public void ReadJsonFile(string path)
        {
            if(SideValidation.FileNameValidation(path, "json"))
            {
                string errors = "";
                containers = new List<Container>();
                ids = new HashSet<string>();
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string text = (File.ReadAllText(projectDirectory + "\\" + path));
                var jarray = JArray.Parse(text);
                foreach (var j in jarray)
                {
                    try
                    {
                        containers.Add(Container.Deserialize(j.ToString()));
                        ids.Add(containers[containers.Count - 1].Id);
                    }
                    catch(ArgumentException e) {
                        errors += "Errors in container!!:\n" + e.Message + "\n";
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
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                List<Dictionary<string, string>> s = new List<Dictionary<string, string>>();
                foreach (var c in containers)
                {
                    s.Add(c.GetDict());
                }
                File.WriteAllText(projectDirectory + "\\" + path, JsonConvert.SerializeObject(s, Formatting.Indented));
                return;

            }
            throw new ArgumentException("filename is invalid");
        }

        public ContainerCollection Search(string search)
        {
            ContainerCollection found = new ContainerCollection();
            foreach(var x in containers)
            {
                foreach(var i in x.GetDict().Values)
                {
                    if (i.ToString().Contains(search))
                    {
                        found.Add(x);
                        break;
                    }
                }
            }
            return found;
        }
        /*sorting_attr = Validation.validate_default_property(sorting_attr, Container.default_props())

            # function helper to lambda
            def get_attr(x):
                attr = getattr(x, sorting_attr)
                if isinstance(attr, str):
                    return attr.lower()
                else:
                    return attr

            self._collection = sorted(self._collection, key=lambda x: get_attr(x), reverse=reverse)*/
        public void Sort(string sorting_attr = "number")
        {
            if (!Container.GetPropsNames().Contains(sorting_attr))
            {
                sorting_attr = "number";
                
            }
            if (sorting_attr == "amount_of_items")
            {
                containers.Sort((a, b) => int.Parse(a.GetDict()[sorting_attr]).CompareTo(int.Parse(b.GetDict()[sorting_attr])));
            }
            else if (sorting_attr == "departure_date" || sorting_attr == "arrival_date")
            {
                containers.Sort((a, b) => DateTime.Parse(a.GetDict()[sorting_attr]).CompareTo(DateTime.Parse(b.GetDict()[sorting_attr])));
            }
            else
            {
                containers.Sort((a, b) => a.GetDict()[sorting_attr].ToLower().CompareTo(b.GetDict()[sorting_attr].ToLower()));
            }
        }

    }
    
}
