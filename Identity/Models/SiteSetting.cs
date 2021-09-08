using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class SiteSetting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? LastTimeChanged { get; set; }
    }
}
