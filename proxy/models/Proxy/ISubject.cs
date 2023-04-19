using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pattern_proxy_np.models.Collection;

namespace pattern_proxy_np.models.Proxy
{
    public interface ISubject
    {
        public Collection<Container> Search(string s);
        public Collection<Container> Sort(string sort_by);
        public Container ViewById(string id);
        public Collection<Container> ViewList();
        public Container Edit(string id, string prop, string new_value);
        public Container Delete(string id);
        public Container Create();
        public void ReadFromJsonFile(string fileName);
        public void WriteToJsonFile(string fileName);
    }
}
