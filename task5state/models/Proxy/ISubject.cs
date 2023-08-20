using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pattern_proxy_np.models.Collection;
using pattern_proxy_np.models.State;

namespace pattern_proxy_np.models.Proxy
{
    public interface ISubject
    {
        public Collection<Container> Search(string s, IState? filter = null);
        public Collection<Container> Sort(string sort_by, IState? filter = null);
        public Container ViewById(string id);
        public Collection<Container> ViewList(IState? filter = null);
        public Container Edit(string id, string prop, string new_value);
        public Container Delete(string id);
        public Container Create();
        public void ReadFromJsonFile(string fileName);
        public void WriteToJsonFile(string fileName);
    }
}
