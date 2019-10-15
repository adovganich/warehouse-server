using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseServer.Model.Enums;

namespace WarehouseServer.Model
{
    public class EventLog
    {
        [Key]
        public string Id { get; set; }
        public EventType Type { get; set; }
        public User User { get; set; }
        public Item Item { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm:ss dd MMM yyyy}")]
        public DateTime Time { get; set; }

    }
}
