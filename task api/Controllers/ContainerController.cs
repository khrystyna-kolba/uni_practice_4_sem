using ContainersApiTask.Migrations;
using ContainersApiTask.Models;
using ContainersApiTask.Models.Containers;
using ContainersApiTask.Models.State;
using ContainersApiTask.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ContainersApiTask.Controllers
{
    [Produces("application/xml")]
    [Authorize]
    public class ContainerController : Controller
    {
        private readonly ILogger<ContainerController> _logger;
        private AppDbContext _context;
        private UserManager<User> _userManager;
        private PermissionProxy _permissionProxy;
        private LoggerManager _loggerManager;
        private ContextManager _contextManager;
        public ContainerController(ILogger<ContainerController> logger, AppDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _contextManager = new ContextManager(context);
            _permissionProxy = new PermissionProxy(_userManager, _contextManager);
            _loggerManager = new LoggerManager(userManager);
        }


        private IActionResult BadRequestErrorMessages()
        {
            var errMsgs = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errMsgs);
        }

        [HttpGet("containers")]
        public async Task<IActionResult>  GetContainers([FromQuery] QueryRequest q)
        {
            var user = await _userManager.GetUserAsync(User);
            _permissionProxy.CurrentUser = user;
            _loggerManager.CurrentUser = user;
            var res = await _permissionProxy.GetContainers(q);
            _loggerManager.MakeLog(res.Item1, nameof(GetContainers), q.ToString());
            return res.Item2;
        }
        [HttpPut("container")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> EditContainer(string Id, [FromBody] ContainerWorkingRequest cont)
        {
            var user = await _userManager.GetUserAsync(User);
            _permissionProxy.CurrentUser = user;
            _loggerManager.CurrentUser = user;
            var res = await _permissionProxy.EditContainer(Id, cont);
            _loggerManager.MakeLog(res.Item1, nameof(EditContainer), $"id of container: {Id}");
            return res.Item2;
        }
        [HttpPost("publish")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PublishContainer(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            _permissionProxy.CurrentUser = user;
            _loggerManager.CurrentUser = user;
            var res = await _permissionProxy.PublishContainer(id);
            _loggerManager.MakeLog(res.Item1, nameof(PublishContainer), $"id of container: {id}");
            return res.Item2;
        }
        [HttpDelete("container")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteContainer(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            _permissionProxy.CurrentUser = user;
            _loggerManager.CurrentUser = user;
            var res = await _permissionProxy.DeleteContainer(id);
            _loggerManager.MakeLog(res.Item1, nameof(DeleteContainer), $"id of container: {id}");
            return res.Item2;
        }

        [HttpGet("container")]
        public async Task<IActionResult> ViewById(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            _permissionProxy.CurrentUser = user;
            _loggerManager.CurrentUser = user;
            var res = await _permissionProxy.ViewById(id);
            _loggerManager.MakeLog(res.Item1, nameof(ViewById), $"id of container: {id}");
            return res.Item2;
        }

        [HttpPost("containers")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddContainer([FromBody] ContainerWorkingRequest cont)
        {
            var user = await _userManager.GetUserAsync(User);
            _permissionProxy.CurrentUser = user;
            _loggerManager.CurrentUser = user;
            var res = await _permissionProxy.AddContainer(cont);
            _loggerManager.MakeLog(res.Item1, nameof(AddContainer));
            return res.Item2;
        }
    }
}
