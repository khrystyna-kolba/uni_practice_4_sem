using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using pattern_proxy_np.models.Authentification;
using pattern_proxy_np.models.Collection;

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
            throw new AccessViolationException("forbidden");
        }
        public bool CheckCustomerAccess()
        {
            if (auth.CurrentUser != null)
            {
                return auth.CurrentUser.Role == Role.customer;
            }
            throw new AccessViolationException("forbidden");
        }
        public Collection<Container>? Search(string s)
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.Search(s);
            }
            return null;
        }
        public Collection<Container> Sort(string sort_by)
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.Sort(sort_by);
            }
            return null;
        }
        public Container ViewById(string id)
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.ViewById(id);
            }
            return null;
        }
        public Collection<Container> ViewList()
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.ViewList();
            }
            return null;
        }
        public Container Edit(string id, string prop, string new_value)
        {
            if (CheckAdminAccess())
            {
                return subject.Edit(id, prop, new_value);
            }
            return null;
        }
        public Container Delete(string id)
        {
            if (CheckAdminAccess())
            {
                return subject.Delete(id);
            }
            return null;
        }
        public Container Create()
        {
            if (CheckAdminAccess())
            {
                return subject.Create();
            }
            return null;
        }
        public void ReadFromJsonFile(string fileName)
        {
            if (CheckAdminAccess())
            {
                subject.ReadFromJsonFile(fileName);
            }
        }
        public void WriteToJsonFile(string fileName)
        {
            if (CheckAdminAccess())
            {
                subject.WriteToJsonFile(fileName);
            }
        }
    }
}
