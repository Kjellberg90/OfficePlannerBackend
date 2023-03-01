using DAL.Models;
using DAL.SQLModels;
using Service.DTO;

namespace Service
{
    public interface IGroupService
    {
        public IEnumerable<SQLGroup> GetGroups();
        public GroupInfoDTO GetGroupInfo(string date, int groupId);
        public void UpdateGroup(int groupId, NewGroupInfoDTO newGroup);
        public void DeleteGroup(int groupId);
        public void Refresh();
        public void AddGroup(AddGroupDTO addGroupDTO);
        public List<WeeklyGroupScheduleDTO> GetWeeklysSchedule(string date, int groupId);
        public GetCurrentWeekDTO GetCurrentWeekAndDay(string date);
    }
}