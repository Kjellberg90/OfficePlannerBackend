using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class AdminRoomOverviewDTO
    {
        public string roomName { get; set; }
        public IEnumerable<string> groupNames { get; set; }
    }
}
