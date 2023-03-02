using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQLModels
{
    public class SQLBooking
    {
        public int Id { get; set; }
        public int DayNr { get; set; }


        [ForeignKey(typeof(SQLGroup))]
        public int? GroupID { get; set; }
        [ManyToOne]
        public SQLGroup? Group { get; set; }


        [ForeignKey(typeof(SQLRoom))]
        public int RoomID { get; set; }
        [ManyToOne]
        public SQLRoom Room { get; set; }


        [ForeignKey(typeof(SQLSchedule))]
        public int ScheduleID { get; set; }
        [ManyToOne]
        public SQLSchedule Schedule { get; set; } 
    }
}
