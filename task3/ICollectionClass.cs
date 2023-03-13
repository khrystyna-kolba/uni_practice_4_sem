using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace np_4sem_proj
{
    public interface ICollectionClass<T> where T : ICollectionClass<T>
    {
        public string Id { get;  set; }
        public string Serialize();
        public abstract static T Deserialize(string json);
        public Dictionary<string, string> GetDict();
        public string GetStrProp(string prop);
        public object GetProp(string prop);
        static abstract public List<string> GetPropsNames();
        public void SetProp(string prop, string value);
        static abstract public T Input();
    }
}
