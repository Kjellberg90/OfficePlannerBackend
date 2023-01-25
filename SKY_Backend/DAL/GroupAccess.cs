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
    }
}
