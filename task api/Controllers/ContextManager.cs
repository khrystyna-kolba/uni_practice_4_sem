using ContainersApiTask.Models.Proxy;
using ContainersApiTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContainersApiTask.Models.Containers;
using Microsoft.AspNetCore.Mvc;
using ContainersApiTask.Models.Enumerations;

namespace ContainersApiTask.Controllers
{
    public class ContextManager : Controller
    {
        private AppDbContext _context;
        public ContextManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(RequestStatus, IActionResult)> GetPublishedContainers(Query? q)
        {
            return (RequestStatus.VIEWED, Ok(FilterByQuery<PublishedContainer>(q, _context.PublishedContainers)));
        }
        public async Task<(RequestStatus, IActionResult)> GetAllContainers(Query? q)
        {
            //_loggerProxy.LogInfo(user, nameof(GetContainers), r.LongCount().ToString());
            return (RequestStatus.VIEWED, Ok(FilterByQuery<Container>(q, _context.Containers)));
        }
        public async Task<(RequestStatus, IActionResult)> ViewAllById(string id)
        {
            var c = _context.Containers.Where(c => c.Id == id).FirstOrDefault();
            if (c == null)
            {
                return (RequestStatus.NOT_FOUND, NotFound());
            }
            return (RequestStatus.VIEWED, Ok(c));
        }
        public async Task<(RequestStatus, IActionResult)> ViewPublishedById(string id)
        {
            var c = _context.PublishedContainers.Where(c => c.Id == id).FirstOrDefault();
            if (c == null)
            {
                return (RequestStatus.NOT_FOUND, NotFound());
            }
            return (RequestStatus.VIEWED, Ok(c));
        }
        public async Task<(RequestStatus, IActionResult)> DeleteContainer(string id)
        {
            var c = _context.Containers.Where(co => co.Id == id).FirstOrDefault();
            if (c is null)
            {
                return (RequestStatus.NOT_FOUND, NotFound($"element with id {id} doesn`t exist"));
            }
            _context.Containers.Remove(c);
            _context.SaveChanges();
            return (RequestStatus.DELETED, Ok(c));
        }
        public async Task<(RequestStatus, IActionResult)> PublishContainer(string id, string role)
        {
            var c = _context.Containers.Where(co => co.Id == id).FirstOrDefault();
            if (c is null)
            {
                return (RequestStatus.NOT_FOUND, NotFound($"element with id {id} doesn`t exsist"));
            }
            c.Publish(role);
            _context.SaveChanges();
            return (RequestStatus.UPDATED, Ok(c));
        }
        public async Task<(RequestStatus, IActionResult)> EditContainer(string Id, ContainerWorkingRequest cont)
        {
            if (cont is null)
            {
                return (RequestStatus.BAD_REQUEST, BadRequest("data was in incorrect format"));
            }
            var entity = _context.Containers.FirstOrDefault(e => e.Id == Id);
            if (entity is not null)
            {
                Container c;
                try
                {
                    c = CreateContainerFromWorkingRequest(cont);
                }
                catch (ArgumentException e)
                {
                    return (RequestStatus.BAD_REQUEST, BadRequest(e.Message));
                }
                entity.State = c.State;
                entity.Number = c.Number;
                entity.DepartureCity = c.DepartureCity;
                entity.ArrivalCity = c.ArrivalCity;
                entity.DepartureDate = c.DepartureDate;
                entity.ArrivalDate = c.ArrivalDate;
                entity.AmountOfItems = c.AmountOfItems;
                _context.SaveChanges();
                return (RequestStatus.UPDATED, Ok(entity));
            }

            return (RequestStatus.NOT_FOUND, NotFound($"element with id {Id} doesn't exist"));
        }
        public async Task<(RequestStatus, IActionResult)> AddContainer(ContainerWorkingRequest cont)
        {
            if (cont is null)
            {
                return (RequestStatus.BAD_REQUEST, BadRequest("data was in incorrect format"));
            }
            Container c;
            try
            {
                c = CreateContainerFromWorkingRequest(cont);
            }
            catch (ArgumentException e)
            {
                return (RequestStatus.BAD_REQUEST, BadRequest(e.Message));
            }
            _context.Containers.Add(c);
            _context.SaveChanges();
            return (RequestStatus.CREATED, Ok(c));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cont"></param>
        /// <exception cref="member">Argument Exception</exception>
        /// <returns></returns>
        public Container CreateContainerFromWorkingRequest(ContainerWorkingRequest cont)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>()
            { {"id", Guid.NewGuid().ToString() },
              {"number", cont.Number},
              {"departure_city" , cont.DepartureCity },
              {"arrival_city", cont.ArrivalCity },
              {"departure_date", cont.DepartureDate },
              {"arrival_date", cont.ArrivalDate},
              {"amount_of_items", cont.AmountOfItems }
            };

            return new Container(dict);
        }
        public IEnumerable<T> FilterByQuery<T>(Query? q, DbSet<T> table) where T : class
        {
            bool search = String.IsNullOrEmpty(q.search) ? false : true;
            bool ord = String.IsNullOrEmpty(q.sortBy) ? false : true;
            bool ord_type = true;
            string ord_prop = "";
            var stringProperties = typeof(T).GetProperties();
            q.search = String.Join("_", q.search.Split());
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
