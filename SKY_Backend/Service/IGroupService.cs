using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IGroupService
    {
        public Group GetGroup(int id);

        public IEnumerable<Group> GetGroups();

        public GroupInfoDTO GetGroupInfo(int id);
    }
}