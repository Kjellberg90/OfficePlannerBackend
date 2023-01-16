using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GroupService : IGroupService
    {
        public Group GetGroup(int id)
        {
            var group = MockData.Instance._groups
                .Where(group => group.Id == id)
                .FirstOrDefault();

            return group;
        }
        public IEnumerable<Group> GetGroups()
        {
            var newList = MockData.Instance._groups;
            return newList;
        }
    }
}
