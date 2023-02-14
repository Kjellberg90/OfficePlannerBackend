using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IGroupAccess
    {
        public List<Group> ReadGroupsData();
        public void PostUpdatedGroup(Group group);
        public void DeleteGroupFromFile(int groupId);
        public void RefreshData();
    }
}