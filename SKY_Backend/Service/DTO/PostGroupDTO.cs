using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class PostGroupDTO
    {
        public string Name { get; set; }
        public int TeamMembers { get; set; }
        public int? BookedRoomId { get; set; }
    }
}
