using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class GroupInfoDTO
    {
        public string Name { get; set; }
        public int GroupSize { get; set; }
        public Room? BookedRoom { get; set; }
    }
}
