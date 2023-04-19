using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using pattern_proxy_np.models.Collection;

namespace pattern_proxy_np.models.Proxy
{
    public class RealSubject : ISubject
    {
        private Collection<Container> containers;
        public RealSubject()
        {
            containers = new Collection<Container>();
        }
        public Collection<Container> Containers { get { return containers; } }
        public Collection<Container> Search(string s)
        {
            return containers.Search(s);
        }
        public Collection<Container> Sort(string sort_by)
        {
            return containers.Sort(sort_by);
        }
        public Container ViewById(string id)
        {
            return containers.GetById(id);
        }
        public Collection<Container> ViewList()
        {
            return containers;
        }
        public Container Edit(string id, string prop, string new_value)
        {
            return containers.EditById(id, prop, new_value);
        }
        public Container Delete(string id)
        {
            return containers.DeleteById(id);
        }
        public Container Create()
        {
            Container u = Container.Input();
            containers.Add(u);
            return u;
        }
        public void ReadFromJsonFile(string fileName)
        {
            containers.ReadJsonFile(fileName);
        }
        public void WriteToJsonFile(string fileName)
        {
            containers.WriteJsonFile(fileName);
        }

    }
}
