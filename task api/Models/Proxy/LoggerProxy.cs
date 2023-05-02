//using pattern_proxy_np.models.Authentification;
using ContainersApiTask.Controllers;
using ContainersApiTask.Models.Containers;
using ContainersApiTask.Models.State;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Identity;

namespace ContainersApiTask.Models.Proxy
{
    public class LoggerProxy 
    {
        //private ContainerController subject;
        //public LoggerProxy(ContainerController subject)
        //{
        //    this.subject = subject;
        //}
        private UserManager<User> _userManager;
        public LoggerProxy(UserManager<User> userManager)
        {
             _userManager = userManager;
        }
        private string LogI(User user, string action, params object[] result)
        {
            string r = "";
            var role = _userManager.GetRolesAsync(user).Result[0];
            foreach (var item in result)
            {
                r += " " + item.ToString();
            }
            return role + " " + user.GetFullName() + " " + action + r.ToString() + "\n";
           
        }
        public void LogInfo(User user, string action, params object[] result)
        {
            Log.Information(LogI(user, action, result));
        }
        public void LogErr(User user, string action, params object[] result)
        {
            Log.Error(LogI(user, action, result));
        }
    }
}
