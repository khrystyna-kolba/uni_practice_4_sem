using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace np_4sem_proj
{
    public class Collection<T> where T : ICollectionClass<T>
    {
        private List<T> containers;
        private HashSet<string> ids;
        public Collection(params T[] conts)
        {
            containers = new List<T>();
            ids = new HashSet<string>();
            foreach (var cont in conts)
            {
                this.Add(cont);
            }

        }
        public void Add(T cont)
        {
            if (CollectionValidation<T>.NewIdValidation(this, cont.Id))
            {
                containers.Add(cont);
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
            foreach (var cont in containers)
            {
                res += cont.ToString() + "\n";
            }
            return res;
        }
        public HashSet<string> GetIds() { return ids; }
        public void DeleteById(string id)
        {
            if (ids.Contains(id))
            {
                foreach (var cont in containers)
                {
                    if (cont.Id == id)
                    {
                        ids.Remove(id);
                        containers.Remove(cont);
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
                foreach (var cont in containers)
                {
                    if (cont.Id == id)
                    {
                        if (prop == "id" && new_value != id)
                        {
                            if (!CollectionValidation<T>.NewIdValidation(this, new_value))
                            {
                                throw new ArgumentException("element with this id already exists");
                            }
                            ids.Remove(id);
                            ids.Add(new_value);
                            cont.SetProp(prop, new_value);
                            return;
                        }
                        cont.SetProp(prop, new_value);
                    }
                }
            }
            else
            {
                throw new ArgumentException("element was not edited, container with that id doesn't exist!");
            }
        }

        public void ReadJsonFile(string path)
        {
            if (SideValidation.FileNameValidation(path, "json"))
            {
                string errors = "";
                containers = new List<T>();
                ids = new HashSet<string>();
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string text = (File.ReadAllText(projectDirectory + "\\" + path));
                var jarray = JArray.Parse(text);
                foreach (var j in jarray)
                {
                    try
                    {
                        containers.Add((T)T.Deserialize(j.ToString()));
                        ids.Add(containers[containers.Count - 1].Id);
                    }
                    catch (ArgumentException e)
                    {
                        errors += "Errors in element!!:\n" + e.Message + "\n";
                    }
                }
                if (errors.Length > 0)
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

        public Collection<T> Search(string search)
        {
            Collection<T> found = new Collection<T>();
            foreach (var x in containers)
            {
                foreach (var i in x.GetDict().Values)
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
        public void Sort(string sorting_attr)
        {
            if (!T.GetPropsNames().Contains(sorting_attr))
            {
                sorting_attr = T.GetPropsNames()[0];

            }
            containers = containers.OrderBy(a => a.GetProp(sorting_attr)).ToList();
        }
      

    }
}
