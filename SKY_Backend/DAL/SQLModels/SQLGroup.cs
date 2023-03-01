using Microsoft.VisualBasic;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQLModels
{
    public class SQLGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int GroupSize { get; set; }
        public string? Department { get; set; }

        [OneToMany]
        public Collection<SQLBooking> Bookings { get; set; }
    }
}
