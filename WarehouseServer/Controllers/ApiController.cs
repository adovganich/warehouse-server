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

namespace WarehouseServer.Controllers
{
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
            var userEmail = principal?.Claims
              .First(c => c.Type == ClaimTypes.Email);
            var items = _context.Items.Where(x => x.User.Email == userEmail.Value).Select(x => new { x.Id, x.Name });
            return new JsonResult(items);
        }

        [HttpGet]
        [Route("items/{id}")]
        public IActionResult GetItemDetails([FromHeader(Name = "Cookie")] string userCookie, [FromRoute] string id)
        {
            var principal = HttpContext.User as ClaimsPrincipal;
            var userEmail = principal?.Claims
              .First(c => c.Type == ClaimTypes.Email);
            var item = _context.Items.Single(x => x.Id == id && x.User.Email == userEmail.Value);
            return new JsonResult(item);
        }

        [HttpPut]
        [Route("items/{id}")]
        public IActionResult GetItemRequest([FromHeader(Name = "Cookie")] string userCookie, [FromRoute] string id)
        {
            var item = _context.Items.Single(x => x.Id == id);
            var principal = HttpContext.User as ClaimsPrincipal;
            var userEmail = principal?.Claims
              .First(c => c.Type == ClaimTypes.Email);
            item.User = _context.Users.Single(x => x.Email == userEmail.Value);
            _context.SaveChanges();
            return new JsonResult(item);
        }

        [HttpDelete]
        [Route("items/{id}")]
        public IActionResult DeleteItemRequest([FromHeader(Name = "Cookie")] string userCookie, [FromRoute] string id)
        {
            var item = _context.Items.Include(i => i.User).Single(x => x.Id == id);
            var principal = HttpContext.User as ClaimsPrincipal;
            var userEmail = principal?.Claims
              .First(c => c.Type == ClaimTypes.Email);
            item.User = null;
            _context.SaveChanges();
            return new JsonResult("ok");
        }
    }
}