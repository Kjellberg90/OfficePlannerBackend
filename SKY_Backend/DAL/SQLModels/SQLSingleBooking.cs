using Microsoft.VisualBasic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQLModels
{
    public class SQLSingleBooking
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [SQLiteNetExtensions.Attributes.ForeignKey(typeof(SQLRoom))]
        public int RoomID { get; set; }
        public int PinCode { get; set; }  
    }
}
