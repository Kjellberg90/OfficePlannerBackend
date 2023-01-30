using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO;

namespace Service
{
    public class GroupService : IGroupService
    {
        private readonly IGroupAccess _groupAccess;

        public GroupService(IGroupAccess groupAccess)
        {
            this._groupAccess = groupAccess;  
        }

        public IEnumerable<string> GetGroups()
        {
            var groupList = _groupAccess.ReadGroupsData();

            var groupNames = groupList.Select(x => x.Name);

            return groupNames;
        }
    }
}
