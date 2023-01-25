using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class RoomInfoDTO
    {
        public string Name { get; set; }
        public int Seats { get; set; }
        public int AvailableSeats { get; set; }
        public string? GroupName { get; set; }
    }
}
