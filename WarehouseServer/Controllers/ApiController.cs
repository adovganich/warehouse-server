using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseServer.Database;
using WarehouseServer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WarehouseServer.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public ApiController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("items")]
        public IActionResult GetItems([FromHeader(Name = "Cookie")] string userCookie)
        {
            var principal = HttpContext.User as ClaimsPrincipal;
            var userId = principal?.Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier);
            var items = _context.Items.Where(x => x.User.Id == userId.Value).Select(x => new { x.Id, x.Name });
            return new JsonResult(items);
        }

        [HttpGet]
        [Route("items/{id}")]
        public IActionResult GetItemDetails([FromHeader(Name = "Cookie")] string userCookie, [FromRoute] string id)
        {
            var principal = HttpContext.User as ClaimsPrincipal;
            var userId = principal?.Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier);
            var item = _context.Items.SingleOrDefault(x => x.Id.ToString() == id && x.User.Id == userId.Value);
            return new JsonResult(item);
        }

        [HttpPut]
        [Route("items/{id}")]
        public IActionResult GetItemRequest([FromHeader(Name = "Cookie")] string userCookie, [FromRoute] string id)
        {
            var item = _context.Items.Include(i => i.User).SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                var principal = HttpContext.User as ClaimsPrincipal;
                var userId = principal?.Claims
                    .First(c => c.Type == ClaimTypes.NameIdentifier);
                if (item.User != null)
                {
                    var returnEvent = new EventLog() { Id = Guid.NewGuid().ToString(), Type = Model.Enums.EventType.Return, Item = item, User = item.User, Time = DateTime.Now };
                    _context.EventLogs.Add(returnEvent);
                }
                var user = _context.Users.Single(x => x.Id == userId.Value);
                item.User = user;
                var getEvent = new EventLog() { Id = Guid.NewGuid().ToString(), Type = Model.Enums.EventType.Get, Item = item, User = user, Time = DateTime.Now };
                _context.EventLogs.Add(getEvent);
                _context.SaveChanges();
                return new JsonResult("ok");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("items/{id}")]
        public IActionResult DeleteItemRequest([FromHeader(Name = "Cookie")] string userCookie, [FromRoute] string id)
        {
            var item = _context.Items.Include(i => i.User).Single(x => x.Id == id);
            if (item != null)
            {
                var principal = HttpContext.User as ClaimsPrincipal;
                var userId = principal?.Claims
                    .First(c => c.Type == ClaimTypes.NameIdentifier);
                if (item.User != null && item.User.Id == userId.Value)
                {
                    var returnEvent = new EventLog() { Id = Guid.NewGuid().ToString(), Type = Model.Enums.EventType.Return, Item = item, User = item.User, Time = DateTime.Now };
                    _context.EventLogs.Add(returnEvent);
                    item.User = null;
                    _context.SaveChanges();
                    return new JsonResult("ok");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}