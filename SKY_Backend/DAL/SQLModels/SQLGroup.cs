using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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
