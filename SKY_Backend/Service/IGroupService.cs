using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IGroupService
    {
        public IEnumerable<Group> GetGroups(); 
        public GroupInfoDTO GetGroupInfo(string date, int groupId);
        public void UpdateGroup(int groupId, NewGroupInfoDTO newGroup);
    }
}