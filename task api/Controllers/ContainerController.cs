using ContainersApiTask.Models;
using ContainersApiTask.Models.Containers;
using ContainersApiTask.Models.Proxy;
using ContainersApiTask.Models.State;
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
        private LoggerProxy _loggerProxy;
        public ContainerController(ILogger<ContainerController> logger, AppDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _permissionProxy = new PermissionProxy(context, userManager);
            _loggerProxy = new LoggerProxy(userManager);
        }


        private IActionResult BadRequestErrorMessages()
        {
            var errMsgs = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errMsgs);
        }

        [HttpGet("containers")]
        public async Task<IActionResult>  GetContainers([FromQuery] Query q)
        {
            var u = await _userManager.GetUserAsync(User);
            //_loggerProxy.LogInfo(await _userManager.GetUserAsync(User), nameof(GetContainers), q.ToString());
            return await _permissionProxy.GetContainers(q, u);
        }
        [HttpPut("container")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> EditContainer(string Id, [FromBody] ContainerWorkingRequest cont)
        { 
            var entity = _context.Containers.FirstOrDefault(e => e.Id == Id);
            if (entity is not null)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>()
            { {"id", Id },
            { "number", cont.Number},
                {"departure_city" , cont.DepartureCity },
                {"arrival_city", cont.ArrivalCity },
                {"departure_date", cont.DepartureDate },
                {"arrival_date", cont.ArrivalDate
    },
                {"amount_of_items", cont.AmountOfItems }
            };
                
                Container c;
                try
                {
                    c = new Container(dict);
                }
                catch (ArgumentException e)
                {
                    _loggerProxy.LogErr(await _userManager.GetUserAsync(User), nameof(AddContainer), e.Message);
                    return BadRequest(e.Message);
                }
                entity.State = c.State;
                entity.Number = c.Number;
                entity.DepartureCity = c.DepartureCity;
                entity.ArrivalCity = c.ArrivalCity;
                entity.DepartureDate = c.DepartureDate;
                entity.ArrivalDate = c.ArrivalDate;
                entity.AmountOfItems = c.AmountOfItems;
                _loggerProxy.LogInfo(await _userManager.GetUserAsync(User), nameof(EditContainer), Id);
                _context.SaveChanges();
                return Ok(entity);
            }

           return NotFound($"element with id {Id} doesn't exist");
        }
        [HttpPost("publish")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PublishContainer(string id)
        {
            _loggerProxy.LogInfo(await _userManager.GetUserAsync(User), nameof(PublishContainer), id);
            var c = _context.Containers.Where(co => co.Id == id).FirstOrDefault();
            var idd = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(idd).Result;
            var role = _userManager.GetRolesAsync(user).Result[0];
            c.Publish(role);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("container")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteContainer(string id)
        {
           
            var c = _context.Containers.Where(co => co.Id == id).FirstOrDefault();
            if (c is null)
            {
                _loggerProxy.LogInfo(await _userManager.GetUserAsync(User), nameof(DeleteContainer), id);
                return NotFound($"element with id {id} doesn`t exsist");
            }
            _context.Containers.Remove(c);
            _loggerProxy.LogInfo(await _userManager.GetUserAsync(User), nameof(DeleteContainer), id);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("container")]
        public async Task<IActionResult> ViewByID(string id)
        {
            return await _permissionProxy.ViewByID(id, _userManager.GetUserAsync(User).Result);
        }

        [HttpPost("containers")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddContainer([FromBody] ContainerWorkingRequest cont)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            //Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(cont);
            Dictionary<string, object> dict = new Dictionary<string, object>()
            { {"id", Guid.NewGuid().ToString() },
            { "number", cont.Number},
                {"departure_city" , cont.DepartureCity },
                {"arrival_city", cont.ArrivalCity },
                {"departure_date", cont.DepartureDate },
                {"arrival_date", cont.ArrivalDate
    },
                {"amount_of_items", cont.AmountOfItems }
            };
            Container c;
            try
            {
                c = new Container(dict);
            }
            catch (ArgumentException e)
            {
                _loggerProxy.LogErr(await _userManager.GetUserAsync(User), nameof(AddContainer), e.Message);
                return BadRequest(e.Message);
            }
            _context.Containers.Add(c);
            _context.SaveChanges();
            _loggerProxy.LogInfo(await _userManager.GetUserAsync(User), nameof(AddContainer), c.Id);
            return Ok();
        }
    }
}
