using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.AdminGroupService
{
    public interface IAdminGroupService
    {
        public void UpdateGroup(int groupId, NewGroupInfoDTO newGroup);
        public void DeleteGroup(int groupId);
        public void AddGroup(AddGroupDTO addGroupDTO);
    }
}
