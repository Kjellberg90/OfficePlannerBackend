using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using DAL.Models;

namespace DAL
{
    public class GroupAccess : IGroupAccess
    {
        public void PostUpdatedGroup(Group group)
        {
            var groupList = ReadGroupsData();

            int index = groupList.FindIndex(g => g.Id == group.Id);

            if (index != -1)
            {
                groupList[index] = group;
            }

            PrintToFile(groupList.OrderBy(g => g.Id));
        }

        public void DeleteGroupFromFile(int groupId)
        {
            var groupList = ReadGroupsData();
            var group = groupList.SingleOrDefault(g => g.Id == groupId);

            if (group != null)
            {
                groupList.Remove(group);
            }

            PrintToFile(groupList);
        }

        public List<Group> ReadGroupsData()
        {
            var groupList = new List<Group>();

            string json;

            using (StreamReader sr = new StreamReader("JsonData/Groups.json"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    groupList = JsonSerializer.Deserialize<List<Group>>(json);

                    return groupList;
                }
                return groupList;
            }
        }
        private void PrintToFile(IEnumerable<object> objects)
        {
            string type = objects.FirstOrDefault().GetType().Name.ToString();

            string printDest = $"JsonData/{type}s.json";

            using (StreamWriter sw = new StreamWriter(printDest))
            {
                var json = JsonSerializer.Serialize(objects);
                sw.WriteLine(json);
            }
        }

        public void RefreshData()
        {
            var mock = new MockData();
            PrintToFile(mock._groups);
        }
    }
}
