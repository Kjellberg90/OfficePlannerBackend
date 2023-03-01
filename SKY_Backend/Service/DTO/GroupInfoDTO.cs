using DAL.Models;
using DAL.SQLModels;
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
        public SQLRoom? BookedRoom { get; set; }
    }
}
