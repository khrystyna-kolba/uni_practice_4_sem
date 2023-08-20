using ContainersApiTask.Models.Containers;
using ContainersApiTask.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainersApiTask.Models.State
{
    public interface IState
    {
        public void Publish(Container cont, string role);
        public void Unpublish(Container cont)
        {
            cont.SetState(new Draft());
        }
    }

    public class Draft : IState
    {
        public void Publish(Container cont, string role)
        {
            if (role == "Manager")
            {
                cont.SetState(new Moderation());
            }
            else if (role == "Admin")
            {
                cont.SetState(new Published());
            }
        }
    }

    public class Moderation : IState
    {
        public void Publish(Container cont, string role)
        {
            if (role == "Admin")
            {
                cont.SetState(new Published());
            }
        }
    }

    public class Published : IState
    {
        public void Publish(Container cont, string role)
        {
            // Is already published
        }
    }
}
