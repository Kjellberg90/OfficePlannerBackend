using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class SingleBookingDTO
    {
        public int RoomId { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
