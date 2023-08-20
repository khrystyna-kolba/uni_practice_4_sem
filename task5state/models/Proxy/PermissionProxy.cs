using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using pattern_proxy_np.models.Authentification;
using pattern_proxy_np.models.Collection;
using pattern_proxy_np.models.State;

namespace pattern_proxy_np.models.Proxy
{
    public class PermissionProxy : ISubject
    {
        private RealSubject subject;
        private Auth auth;
        public PermissionProxy(RealSubject subject, Auth auth)
        {
            this.subject = subject;
            this.auth = auth;
        }
        public Collection<Container> Containers { get { return subject.Containers; } }
        public User CurrentUser { get => auth.CurrentUser; }
        public bool CheckAdminAccess()
        {
            if (auth.CurrentUser != null)
            {
                return auth.CurrentUser.Role == Role.admin;
            }
            throw new AccessViolationException("unauthorized");
        }
        public bool CheckManagerAccess()
        {
            if (auth.CurrentUser != null)
            {
                return auth.CurrentUser.Role == Role.manager;
            }
            throw new AccessViolationException("unauthorized");
        }
        public bool CheckCustomerAccess()
        {
            if (auth.CurrentUser != null)
            {
                return auth.CurrentUser.Role == Role.customer;
            }
            throw new AccessViolationException("unauthorized");
        }
        public Container Publish(string id)
        {
            if (CheckCustomerAccess() || CheckAdminAccess() || CheckManagerAccess())
            {
                return subject.Publish(id, CurrentUser);
            }
            throw new AccessViolationException("forbidden");
        }
        public Collection<Container>? Search(string s, IState? filter = null)
        {
            if (CheckCustomerAccess())
            {
                if (filter is not Published)
                {
                    throw new AccessViolationException("forbidden");
                }
                return subject.Search(s, filter);
            }
            else if (CheckAdminAccess() || CheckManagerAccess())
            {
                return subject.Search(s, filter);
            }
            throw new AccessViolationException("forbidden");
        }
        public Collection<Container> Sort(string sort_by, IState? filter = null)
        {
            if (CheckCustomerAccess())
            {
                return subject.Sort(sort_by, filter);
            }
            else if (CheckAdminAccess() || CheckManagerAccess())
            {
                return subject.Sort(sort_by, filter);
            }
            throw new AccessViolationException("forbidden");
        }
        public Container ViewById(string id)
        {
            if (CheckCustomerAccess())
            {
                var res = subject.ViewById(id);
                if (res.GetState() is not Published)
                {
                    throw new AccessViolationException("forbidden");
                }
                return res;
            }
            else if (CheckAdminAccess() || CheckManagerAccess())
            {
                return subject.ViewById(id);
            }
            throw new AccessViolationException("forbidden");
        }
        public Collection<Container> ViewList(IState? filter = null)
        {
            if (CheckCustomerAccess())
            {
                if (filter is not Published)
                {
                    throw new AccessViolationException("forbidden");
                }
                return subject.ViewList(filter);
            }
            else if (CheckAdminAccess() || CheckManagerAccess())
            {
                return subject.ViewList(filter);
            }
            throw new AccessViolationException("forbidden");
        }
        public Container Edit(string id, string prop, string new_value)
        {
            if (CheckAdminAccess() || CheckManagerAccess())
            {
                return subject.Edit(id, prop, new_value);
            }
            throw new AccessViolationException("forbidden");
        }
        public Container Delete(string id)
        {
            if (CheckAdminAccess())
            {
                return subject.Delete(id);
            }
            throw new AccessViolationException("forbidden");
        }
        public Container Create()
        {
            if (CheckAdminAccess() || CheckManagerAccess())
            {
                return subject.Create();
            }
            throw new AccessViolationException("forbidden");
        }
        public void ReadFromJsonFile(string fileName)
        {
            if (CheckAdminAccess() || CheckManagerAccess())
            {
                subject.ReadFromJsonFile(fileName);
                return;
            }
            throw new AccessViolationException("forbidden");
        }
        public void WriteToJsonFile(string fileName)
        {
            if (CheckAdminAccess())
            {
                subject.WriteToJsonFile(fileName);
                return;
            }
            throw new AccessViolationException("forbidden");
        }
    }
}
