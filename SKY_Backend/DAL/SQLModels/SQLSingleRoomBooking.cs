using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQLModels
{
    public class SQLSingleRoomBooking
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DayNr { get; set; }

        [ForeignKey(typeof(SQLGroup))]
        public int GroupID { get; set; }

        [ForeignKey(typeof(SQLRoom))]
        public int RoomID { get; set;}
    }
}
