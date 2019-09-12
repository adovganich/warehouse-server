using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseServer.Database;
using WarehouseServer.Model;

namespace WarehouseServer.Controllers
{
    [Route("[controller]")]
    public class ItemsController : Controller
    {
        private readonly WarehouseContext _context;

        public ItemsController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Items.Include(e => e.User).ToListAsync());
        }
    }
}
