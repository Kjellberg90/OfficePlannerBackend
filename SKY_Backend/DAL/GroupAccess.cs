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
        public List<Group> ReadGroupsData()
        {
            var groupsList = new List<Group>();

            string json;

            using (StreamReader sr = new StreamReader("JsonData/Groups.json"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    groupsList = JsonSerializer.Deserialize<List<Group>>(json);

                    return groupsList;
                }
                return groupsList;
            }
        }

        public void PrintGroupToFile(Group newGroup)
        {
            var groupsList = ReadGroupsData();

            groupsList.Add(newGroup);

            PrintToFile(groupsList);
        }

        public void DeleteGroupFromFile(int groupId)
        {
            var groupsList = ReadGroupsData();

            groupsList.RemoveAll(group => group.Id == groupId);

            PrintToFile(groupsList);
        }

        public void UpdateGroupOnFile(Group newGroupData)
        {
            var groupsList = ReadGroupsData();

            var group = groupsList
                .Where(group => group.Id == newGroupData.Id)
                .FirstOrDefault();

            if (group == null)
            {
                throw new Exception("Room not found");
            }

            groupsList.FirstOrDefault(room => room.Id == newGroupData.Id).Name = newGroupData.Name;
            groupsList.FirstOrDefault(room => room.Id == newGroupData.Id).TeamMembers = newGroupData.TeamMembers;
            groupsList.FirstOrDefault(room => room.Id == newGroupData.Id).BookedRoomNumber = newGroupData.BookedRoomNumber;

            PrintToFile(groupsList);
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
    }
}
