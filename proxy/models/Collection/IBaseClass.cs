//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pattern_proxy_np.models.Collection
{
    public interface IBaseClass<T> where T : IBaseClass<T>
    {
        public string Id { get; set; }
        public static abstract T FromDict(Dictionary<string, object> dict);
    }
}
