﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace ContainerUniversalTest
{
    public class Collection<T> where T : IBaseClass<T>
    {
        private List<T> collection;
        private HashSet<string> ids;
        public Collection(params T[] conts)
        {
            collection = new List<T>();
            ids = new HashSet<string>();
            foreach (var cont in conts)
            {
                Add(cont);
            }

        }
        public void Add(T cont)
        {
            if (CollectionValidation<T>.NewIdValidation(this, cont.Id))
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
        public HashSet<string> GetIds() { return ids; }
        public void DeleteById(string id)
        {
            if (ids.Contains(id))
            {
                foreach (var cont in collection)
                {
                    if (cont.Id == id)
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
            try
            {
                if (ids.Contains(id))
                {
                    foreach (var cont in collection)
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
                                typeof(T).GetProperty(prop.ToPascalCase()).SetValue(this, Convert.ChangeType(new_value, typeof(T).GetProperty(prop.ToPascalCase()).PropertyType));
                                return;
                            }
                            if (typeof(T).GetProperty(prop.ToPascalCase()).PropertyType.IsEnum)
                            {
                                typeof(T).GetProperty(prop.ToPascalCase()).SetValue(cont, new_value.ParseCity());
                            }
                            else
                            {
                                typeof(T).GetProperty(prop.ToPascalCase()).SetValue(cont, Convert.ChangeType(new_value, typeof(T).GetProperty(prop.ToPascalCase()).PropertyType));
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("element was not edited, element with that id doesn't exist!");
                }
            }
            catch (TargetInvocationException e)
            {
                throw new ArgumentException("element was not edited " + e.InnerException.Message);
            }
        }

        public void ReadJsonFile(string path)
        {
            if (SideValidation.FileNameValidation(path, "json") && SideValidation.FileExist(path))
            {

                string errors = "";
                collection = new List<T>();
                ids = new HashSet<string>();
                string text = SideValidation.ReadFile(path);
                List<Dictionary<string, object>> c = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(text);
                foreach (var j in c)
                {
                    try
                    {
                        collection.Add(T.FromDict(j));
                        ids.Add(collection[collection.Count - 1].Id);
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
                var options = new JsonSerializerOptions { WriteIndented = true };
                SideValidation.WriteFile(path, System.Text.Json.JsonSerializer.Serialize(collection, options));
                return;

            }
            throw new ArgumentException("filename is invalid");
        }

        public Collection<T> Search(string search)
        {
            Collection<T> found = new Collection<T>();
            foreach (var x in collection)
            {
                foreach (var p in typeof(T).GetProperties())
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
            if (!typeof(T).GetProperties().Any(x => x.Name == sorting_attr))
            {
                sorting_attr = typeof(T).GetProperties()[0].Name;
            }

            if (typeof(T).GetProperty(sorting_attr).GetType().IsEnum)
            {
                collection.Sort((a, b) => (Enum.GetName(typeof(City), (City)typeof(T).GetProperty(sorting_attr).GetValue(b)).CompareTo(Enum.GetName(typeof(City), (City)typeof(T).GetProperty(sorting_attr).GetValue(b)))));
            }
            else
            {
                collection = collection.OrderBy(a => typeof(T).GetProperty(sorting_attr).GetValue(a)).ToList();
            }
        }

    }
}
