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
        public Collection<Container>? Search(string s)
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.Search(s);
            }
            throw new AccessViolationException("forbidden");
        }
        public Collection<Container> Sort(string sort_by)
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.Sort(sort_by);
            }
            throw new AccessViolationException("forbidden");
        }
        public Container ViewById(string id)
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.ViewById(id);
            }
            throw new AccessViolationException("forbidden");
        }
        public Collection<Container> ViewList()
        {
            if (CheckCustomerAccess() || CheckAdminAccess())
            {
                return subject.ViewList();
            }
            throw new AccessViolationException("forbidden");
        }
        public Container Edit(string id, string prop, string new_value)
        {
            if (CheckAdminAccess())
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
            if (CheckAdminAccess())
            {
                return subject.Create();
            }
            throw new AccessViolationException("forbidden");
        }
        public void ReadFromJsonFile(string fileName)
        {
            if (CheckAdminAccess())
            {
                subject.ReadFromJsonFile(fileName);
            }
            throw new AccessViolationException("forbidden");
        }
        public void WriteToJsonFile(string fileName)
        {
            if (CheckAdminAccess())
            {
                subject.WriteToJsonFile(fileName);
            }
            throw new AccessViolationException("forbidden");
        }
    }
}
