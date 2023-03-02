using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DAL.SQLModels
{
    public class SQLRoom
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Seats { get; set; }

        [OneToMany]
        public Collection<SQLBooking> Bookings { get; set; }

        [OneToMany]
        public Collection<SQLSingleBooking> SingleBookings { get; set; }
    }
}
