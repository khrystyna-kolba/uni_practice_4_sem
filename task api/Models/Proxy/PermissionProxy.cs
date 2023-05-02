using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ContainersApiTask.Controllers;
//using ContainersApiTask.Models.Authentification;
using ContainersApiTask.Models.Containers;
using ContainersApiTask.Models.State;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ContainersApiTask.Models.Proxy
{
    public class PermissionProxy: Controller
    {
        private AppDbContext _context;
        private UserManager<User> _userManager;
        private LoggerProxy _loggerProxy;
        public PermissionProxy(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _loggerProxy = new LoggerProxy(userManager);
        }
        public async Task<IActionResult> GetContainers([FromQuery] Query? q, User user)
        {
            
            var role = _userManager.GetRolesAsync(user).Result[0];

            
           
            if (role == "Customer")
            {
                var r = FilterByQuery<PublishedContainer>(q, _context.PublishedContainers);
                _loggerProxy.LogInfo(user, nameof(GetContainers), r.LongCount().ToString());
                return Ok(r);
            }
            else
            {
                var r = FilterByQuery<Container>(q, _context.Containers);
                _loggerProxy.LogInfo(user, nameof(GetContainers), r.LongCount().ToString());
                return Ok(r);
            }
        }

        public async Task<IActionResult> ViewByID(string id, User user)
        {
            
            var role = _userManager.GetRolesAsync(user).Result[0];
            if (role == "Customer")
            {
                PublishedContainer res = _context.PublishedContainers.Where(c => c.Id == id).FirstOrDefault();
                if (res is null)
                {
                    _loggerProxy.LogErr(user, nameof(ViewByID), id);
                    return NotFound();
                }
                else
                {
                    _loggerProxy.LogInfo(user, nameof(ViewByID), id);
                    return Ok(res);
                }
            }
            Container ress = _context.Containers.Where(c => c.Id == id).FirstOrDefault();
            if (ress is null)
            {
                _loggerProxy.LogErr(user, nameof(ViewByID), id);
                return NotFound();
            }

            _loggerProxy.LogInfo(user, nameof(ViewByID), id);
            return  Ok(ress);
        }

        public IEnumerable<T> FilterByQuery<T>(Query? q, DbSet<T> table) where T : class
        {
            bool search = String.IsNullOrEmpty(q.search) ? false : true;
            bool ord = String.IsNullOrEmpty(q.sortBy) ? false : true;
            bool ord_type = true;
            string ord_prop = "";
            var stringProperties = typeof(T).GetProperties();//.Where(prop =>
//prop.PropertyType == q.GetType());

            


            if (ord)
            {
                ord_type = q.sortBy.StartsWith("-") ? false : true;
                ord_prop = q.sortBy.StartsWith("-") ? q.sortBy.Substring(1) : q.sortBy;
            }
            foreach (var f in stringProperties)
            {
                Console.WriteLine(f.Name);
            }
            ord = stringProperties.Any(prop => prop.Name == ord_prop);
            if (search && ord)
            {
                if (ord_type)
                {
                    return table.AsEnumerable().OrderBy(c => c.GetType().GetProperty(ord_prop).GetValue(c)).Where(c =>
        stringProperties.Any(prop => ((prop.GetValue(c, null) == null) ? "" : prop.GetValue(c).ToString().ToLower()).Contains(q.search.ToLower()))).ToList();
                }
                else
                {
                    return table.AsEnumerable().OrderByDescending(c => c.GetType().GetProperty(ord_prop).GetValue(c)).Where(c =>
        stringProperties.Any(prop => ((prop.GetValue(c, null) == null) ? "" : prop.GetValue(c).ToString().ToLower()).Contains(q.search.ToLower()))).ToList();
                }
            }
            else if (search)
            {
                foreach (var c in table.AsEnumerable())
                {
                    Console.WriteLine(stringProperties.Any(prop =>
                ((prop.GetValue(c, null) == null) ? "" : prop.GetValue(c).ToString().ToLower()).Contains(q.search.ToLower())));
                }
                return table.AsEnumerable().Where(c => stringProperties.Any(prop =>
                ((prop.GetValue(c, null) == null) ? "" : prop.GetValue(c).ToString().ToLower()).Contains(q.search.ToLower()))).ToList();
            }
            else if (ord)
            {
                if (ord_type)
                {
                    return table.AsEnumerable().OrderBy(c => c.GetType().GetProperty(ord_prop).GetValue(c)).ToList();
                }
                else
                {
                    return table.AsEnumerable().OrderByDescending(c => c.GetType().GetProperty(ord_prop).GetValue(c)).ToList();

                }
            }
            else
            {
                return table;
            }

        }
    }
}
