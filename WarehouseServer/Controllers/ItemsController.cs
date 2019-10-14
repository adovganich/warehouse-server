using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
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

        [HttpGet, ActionName("Index")]
        [Route("all")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Items.Include(e => e.User).ToListAsync());
        }

        [HttpGet, ActionName("Create")]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [Route("create")]
        public async Task<IActionResult> Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpGet, ActionName("Delete")]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("UploadFile")]
        [Route("all")]
        public IActionResult UploadFile(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var xdoc = XDocument.Load(formFile.OpenReadStream());
                    var childElements = xdoc.Root.Elements();
                    foreach (var childrenNode in childElements)
                    {
                        var item = new Item();
                        var itemProperties = childrenNode.Elements();
                        foreach (var property in itemProperties)
                        {
                            if (property.Name == "id")
                            {
                                item.Id = property.Value;
                                continue;
                            }
                            if (property.Name == "name")
                            {
                                item.Name = property.Value;
                                continue;
                            }
                        }
                        _context.Items.Add(item);
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("DownloadFile")]
        [Route("download")]
        public IActionResult DownloadFile()
        {
            XDocument xdoc = new XDocument();
            XElement itemsXml = new XElement("items");
            var items = _context.Items.ToList();
            foreach(var item in items)
            {
                XElement element = new XElement("item");
                element.Add(new XElement("id", item.Id));
                element.Add(new XElement("name", item.Name));
                itemsXml.Add(element); 
            }
            xdoc.Add(itemsXml);
            return File(Encoding.UTF8.GetBytes(xdoc.ToString()), "text/xml", "items.xml");
        }
    }
}
