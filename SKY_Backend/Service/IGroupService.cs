using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IGroupService
    {
        public Group GetGroup(int id);
        public IEnumerable<Group> GetGroups();

        public GroupInfoDTO GetGroupInfo(int id);
        public void PostGroup(PostGroupDTO data);
        public void DeleteGroup(int id);
        public void UpdateGroup(Group group);
    }
}