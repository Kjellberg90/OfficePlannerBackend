using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int DayNr { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
