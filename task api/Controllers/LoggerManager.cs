//using pattern_proxy_np.models.Authentification;
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
using ContainersApiTask.Models.Enumerations;
using ContainersApiTask.Models.Extensions;
using ContainersApiTask.Models;

namespace ContainersApiTask.Controllers
{
    public class LoggerManager
    {
        private User? _currentUser;
        private string? role;
        private UserManager<User> _userManager;
        public LoggerManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public User CurrentUser
        {
            set
            {
                _currentUser = value;
                role = _userManager.GetRolesAsync(_currentUser).Result[0];
            }
        }
        private string LogI(RequestStatus s, string action, params object[] result)
        {
            string r = "";
            foreach (var item in result)
            {
                r += " " + item.ToString();
            }
            return s.GetName() + " by " + role + " " + _currentUser.GetFullName() + " executed " + action + "\n" + r.ToString() + "\n";

        }
        public void MakeLog(RequestStatus s, string action, params object[] result)
        {
            if (s > 0)
            {
                Log.Information(LogI(s, action, result));
            }
            else
            {
                Log.Error(LogI(s, action, result));
            }
        }
    }
}
