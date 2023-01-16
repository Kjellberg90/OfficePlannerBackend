using DAL.Models;

namespace Service
{
    public interface IGroupService
    {
        public Group GetGroup(int id);

        public IEnumerable<Group> GetGroups();
    }
}