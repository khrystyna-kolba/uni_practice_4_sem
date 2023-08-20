using pattern_proxy_np.models.Authentification;
using pattern_proxy_np.models.Collection;
using pattern_proxy_np.models.State;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pattern_proxy_np.models.Proxy
{
    public class LoggerProxy : ISubject
    {
        private static string logFileName = "log.txt";
        private PermissionProxy subject;
        public LoggerProxy(PermissionProxy subject)
        {
            this.subject = subject;
        }
        private void Log(string user, DateTime date, string action, params object[] result)
        {
            string r = "";
            foreach (var item in result)
            {
                r += " " + item.ToString();
            }
            string res = user + " " + date.ToString() + " " + action + r.ToString() + "\n";
            SideValidation.WriteToEndFile(logFileName, res);
        }
        public Container Publish(string id)
        {
            Container c = subject.Publish(id);
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(Publish), subject.CurrentUser.Role.GetName(), $"id: {id}", "\n", c);
            return c; 
        }
        public Collection<Container> Search(string search, IState? filter = null)
        {
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(Search), filter.GetType().Name, $"search: {search}", "\n", subject.Search(search, filter));
            return subject.Search(search, filter);
        }
        public Collection<Container> Sort(string sort_by, IState? filter = null)
        {
            Collection<Container> r = subject.Sort(sort_by);
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(Sort), filter.GetType().Name, $"sort_by: {sort_by}", "\n", r);
            return r;
        }
        public Container ViewById(string id)
        {
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(ViewById), $"id: {id}", "\n", subject.ViewById(id));
            return subject.ViewById(id);
        }
        public Collection<Container> ViewList(IState? filter = null)
        {
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(ViewList), "\n", filter.GetType().Name, subject.ViewList(filter));
            return subject.ViewList(filter);
        }
        public Container Edit(string id, string prop, string new_value)
        {
            string b = subject.ViewById(id).ToString();
            Container r = subject.Edit(id, prop, new_value);
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(Edit), $"id: {id}", $"prop: {prop}", "\n", b, "---->", r);
            return r;
        }
        public Container Delete(string id)
        {
            Container r = subject.Delete(id);
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(Delete), $"id: {id}", "\n", r);
            return r;
        }
        public Container Create()
        {
            Container r = subject.Create();
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(Create), "\n", r);
            return r;
        }
        public void ReadFromJsonFile(string fileName)
        {
            subject.ReadFromJsonFile(fileName);
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(ReadFromJsonFile), $"file: {fileName}", subject.ViewList());
        }
        public void WriteToJsonFile(string fileName)
        {
            subject.WriteToJsonFile(fileName);
            Log(subject.CurrentUser.GetFullName(), DateTime.Now, nameof(WriteToJsonFile), $"file: {fileName}", subject.ViewList());
        }
    }
}
