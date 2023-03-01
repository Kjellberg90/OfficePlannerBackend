using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQLModels
{
    public class SQLSingleBooking
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(typeof(SQLRoom))]
        public int RoomID { get; set; }
    }
}
