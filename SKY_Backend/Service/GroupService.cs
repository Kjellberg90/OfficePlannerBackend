using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO;

namespace Service
{
    public class GroupService : IGroupService
    {
        private readonly IRoomService _roomService;
        private readonly IGroupAccess _groupAccess;

        public GroupService(IRoomService roomService, IGroupAccess groupAccess)
        {
            _roomService = roomService;
            this._groupAccess = groupAccess;  
        }

        
    }
}
