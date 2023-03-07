using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class GroupToRoomBookingDTO
    {
        public int GroupId { get; set; }
        public int RoomId { get; set; }
        public string Date { get; set; }
    }
}
