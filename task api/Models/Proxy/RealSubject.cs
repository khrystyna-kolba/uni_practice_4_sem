using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ContainersApiTask.Models.Containers;
using ContainersApiTask.Models.State;

namespace ContainersApiTask.Models.Proxy
{
    //public class RealSubject : ISubject
    //{
    //    private Collection<Container> containers;
    //    public RealSubject()
    //    {
    //        containers = new Collection<Container>();
    //    }
    //    public Collection<Container> Containers { get { return containers; } }
    //    public Container Publish(string id, User user)
    //    {
    //        Container c = containers.GetById(id);
    //        c.Publish(user);
    //        return c;
    //    }
        
    //    public Collection<Container> Search(string s, IState? filter = null)
    //    {
    //        if (filter is null)
    //        {
    //            return containers.Search(s);
    //        }

    //        Collection<Container> res = new Collection<Container>();
    //        foreach (Container container in containers)
    //        {
    //            if (container.GetState().GetType() == filter.GetType())
    //            {
    //                res.Add(container);
    //            }
    //        }
    //        return res.Search(s);
    //    }
    //    public Collection<Container> Sort(string sort_by, IState? filter = null)
    //    { 
    //        containers.Sort(sort_by);
    //        return ViewList(filter);
    //    }
    //    public Container ViewById(string id)
    //    {
    //        return containers.GetById(id);
    //    }
    //    public Collection<Container> ViewList(IState? filter = null)
    //    {
    //        if (filter is null)
    //        {
    //            return containers;
    //        }

    //        Collection<Container> res = new Collection<Container>();
    //        foreach (Container container in containers)
    //        {
    //            if (container.GetState().GetType() == filter.GetType())
    //            {
    //                res.Add(container);
    //            }
    //        }
    //        return res;
    //    }
    //    public Container Edit(string id, string prop, string new_value)
    //    {
    //        return containers.EditById(id, prop, new_value);
    //    }
    //    public Container Delete(string id)
    //    {
    //        return containers.DeleteById(id);
    //    }
    //    public Container Create()
    //    {
    //        Container u = Container.Input();
    //        containers.Add(u);
    //        return u;
    //    }
    //    public void ReadFromJsonFile(string fileName)
    //    {
    //        containers.ReadJsonFile(fileName);
    //    }
    //    public void WriteToJsonFile(string fileName)
    //    {
    //        containers.WriteJsonFile(fileName);
    //    }

    //}
}
