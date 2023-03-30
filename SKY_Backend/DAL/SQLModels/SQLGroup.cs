using Microsoft.VisualBasic;
using SQLite;
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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int GroupSize { get; set; }
        public string? Department { get; set; }

        [OneToMany]
        public Collection<SQLBooking> Booking { get; set; }

        [OneToMany]
        public Collection<SQLSingleRoomBooking> SingleRoomBooking { get; set; }
    }
}
