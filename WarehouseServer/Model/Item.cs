using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseServer.Model
{
    public class Item
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
    }
}
