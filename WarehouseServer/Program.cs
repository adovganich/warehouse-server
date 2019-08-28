using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WarehouseServer.Database;
using WarehouseServer.Model;
using Microsoft.EntityFrameworkCore;

namespace WarehouseServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (var db = new WarehouseContext())
            //{
            //    db.Database.Migrate();
            //    db.Items.Add(new Item { Id = "1111", Name = "bbbbb" });
            //    db.Users.Add(new User { Email = "3333", Password = "aaaaa" });
            //    var count = db.SaveChanges();
            //    Console.WriteLine("{0} records saved to database", count);

            //    Console.WriteLine();
            //    Console.WriteLine("All blogs in database:");
            //    foreach (var item in db.Items)
            //    {
            //        Console.WriteLine(" - {0}", item.Name);
            //    }
            //}
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
