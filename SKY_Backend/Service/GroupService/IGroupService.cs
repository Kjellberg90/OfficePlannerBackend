using DAL.Models;
using DAL.SQLModels;
using Service.DTO;

namespace Service.GroupService
{
    public interface IGroupService
    {
        public IEnumerable<SQLGroup> GetGroups();
        public GroupInfoDTO GetGroupInfo(string date, int groupId);

        public List<WeeklyGroupScheduleDTO> GetWeeklysSchedule(string date, int groupId);
        public GetCurrentWeekDTO GetCurrentWeekAndDay(string date);
    }
}