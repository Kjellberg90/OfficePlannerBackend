using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IGroupService
    {
        public Group GetGroup(int id);

        public GroupInfoDTO GetGroupInfo(int id);
        public void PrintGroupToFile(string data);
    }
}