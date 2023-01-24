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
        public int? United { get; set; }
        public int? Innovation { get; set; }
        public int? Committment { get; set; }
        public int? Collaboration { get; set; }
        public int? InspiredA { get; set; }
        public int? InspiredB { get; set; }

    }
}
