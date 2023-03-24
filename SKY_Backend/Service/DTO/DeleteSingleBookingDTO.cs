using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class DeleteSingleBookingDTO
    {
        public string date { get; set; }
        public string name { get; set; }
        public int roomId { get; set; }
        public string password { get; set; }
    }
}
