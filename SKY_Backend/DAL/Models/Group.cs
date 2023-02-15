using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupSize { get; set; }
        public string? Division { get; set; }
    }
}
