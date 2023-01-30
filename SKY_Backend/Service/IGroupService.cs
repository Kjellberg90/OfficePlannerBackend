using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IGroupService
    {
        public IEnumerable<Group> GetGroups(); 
        public GroupInfoDTO GetGroupInfo(string date, int groupId);
    }
}