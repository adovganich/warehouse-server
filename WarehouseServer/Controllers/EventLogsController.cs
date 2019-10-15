using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseServer.Database;
using WarehouseServer.Model;

namespace WarehouseServer.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EventLogsController : Controller
    {
        private readonly WarehouseContext _context;

        public EventLogsController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventLogs.Include(e => e.User).Include(e => e.Item).ToListAsync());
        }
    }
}
