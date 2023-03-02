using DAL.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class UpdateBookingsDTO
    {
        public string roomName { get; set; } = null!;
        public string[] groupNames { get; set; } = null!;
    }
}
