using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ContainersApiTask.Controllers;
using ContainersApiTask.Migrations;
using ContainersApiTask.Models;
//using ContainersApiTask.Models.Authentification;
using ContainersApiTask.Models.Containers;
using ContainersApiTask.Models.Enumerations;
using ContainersApiTask.Models.State;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ContainersApiTask.Proxy
{
    public class PermissionProxy : Controller
    {
        private UserManager<User> _userManager;
        private string? role;
        private User? _currentUser;
        private ContextManager _contextManager;
        public PermissionProxy(UserManager<User> userManager, ContextManager contextManager)
        {
            _userManager = userManager;
            _contextManager = contextManager;
        }
        public User CurrentUser
        {
            set
            {
                _currentUser = value;
                role = _userManager.GetRolesAsync(_currentUser).Result[0];
            }
        }
        public string Role { get => role; }
        public async Task<(RequestStatus, IActionResult)> EditContainer(string Id, ContainerWorkingRequest cont)
        {
            if (role == "Admin" || role == "Manager")
            {
                return await _contextManager.EditContainer(Id, cont);
            }

            return (RequestStatus.FORBIDDEN, Forbid());
        }

        public async Task<(RequestStatus, IActionResult)> PublishContainer(string id)
        {
            if (role == "Admin" || role == "Manager")
            {
                return await _contextManager.PublishContainer(id, role);
            }

            return (RequestStatus.FORBIDDEN, Forbid());
        }
        public async Task<(RequestStatus, IActionResult)> DeleteContainer(string id)
        {
            if (role == "Admin")
            {
                return await _contextManager.DeleteContainer(id);
            }
            return (RequestStatus.FORBIDDEN, Forbid());
        }
        public async Task<(RequestStatus, IActionResult)> AddContainer(ContainerWorkingRequest cont)
        {
            if (role == "Admin" || role == "Manager")
            {
                return await _contextManager.AddContainer(cont);
            }
            return (RequestStatus.FORBIDDEN, Forbid());
        }
        public async Task<(RequestStatus, IActionResult)> GetContainers(Query? q)
        {
            if (role == "Admin" || role == "Manager")
            {
                return await _contextManager.GetAllContainers(q);
            }
            else
            {
                return await _contextManager.GetPublishedContainers(q);
            }
        }

        public async Task<(RequestStatus, IActionResult)> ViewById(string id)
        {
            if (role == "Admin" || role == "Manager")
            {
                return await _contextManager.ViewAllById(id);
            }
            else
            {
                return await _contextManager.ViewPublishedById(id);
            }
        }


    }
}
