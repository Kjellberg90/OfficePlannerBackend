using DAL;
using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.AdminGroupService
{
    public class AdminGroupService : IAdminGroupService
    {

        public void UpdateGroup(int groupId, NewGroupInfoDTO newGroup)
        {
            using (var context = new SkyDbContext())
            {
                var group = context.Groups
                    .Where(g => g.Id == groupId)
                    .FirstOrDefault();

                if (group == null) throw new Exception("Group not found");

                group.Name = newGroup.Name;
                group.GroupSize = newGroup.GroupSize;
                if (newGroup.Division != null) group.Department = newGroup.Division;

                context.SaveChanges();
            }
        }

        public void DeleteGroup(int groupId)
        {
            using (var context = new SkyDbContext())
            {
                var group = context.Groups.FirstOrDefault(r => r.Id == groupId);
                if (group == null) throw new ArgumentNullException(nameof(group));

                context.Groups.Remove(group);
                context.SaveChanges();
            }
        }

        public void AddGroup(AddGroupDTO addGroupDTO)
        {
            using (var context = new SkyDbContext())
            {
                context.Groups.Add(new SQLGroup
                {
                    Name = addGroupDTO.Name,
                    GroupSize = addGroupDTO.GroupSize,
                    Department = addGroupDTO.Division
                });

                context.SaveChanges();
            }
        }
    }
}
