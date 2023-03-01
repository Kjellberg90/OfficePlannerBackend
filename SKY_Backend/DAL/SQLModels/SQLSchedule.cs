using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQLModels
{
    public class SQLSchedule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WeekInterval { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public Collection<SQLBooking> Bookings { get; set; }
    }
}
