using DAL.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class GroupBookedToRoom
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string GroupName { get; set; }
        public string Date { get; set; }
    }
}
