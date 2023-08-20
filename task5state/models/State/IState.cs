using pattern_proxy_np.models.Collection;
using pattern_proxy_np.models.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pattern_proxy_np.models.State
{
    public interface IState
    {
        public void Publish(Container cont, User user);
        public void Unpublish(Container cont, User user) 
        {
            if (user != null) 
            {
                cont.SetState(new Draft());
            }
        }
    }

    public class Draft : IState
    {
        public void Publish(Container cont, User user)
        {
            if (user.Role == Role.manager)
            {
                cont.SetState(new Moderation());
            }
            else if (user.Role == Role.admin)
            {
                cont.SetState(new Published());
            }
        }
    }

    public class Moderation : IState
    {
        public void Publish(Container cont, User user)
        {
            if (user.Role == Role.admin)
            {
                cont.SetState(new Published());
            }
        }
    }

    public class Published : IState
    {
        public void Publish(Container cont, User user)
        {
            return;
        }
    }
}
