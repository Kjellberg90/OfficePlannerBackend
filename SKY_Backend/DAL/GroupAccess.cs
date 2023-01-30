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
    }
}
