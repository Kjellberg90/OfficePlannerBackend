using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DAL.MockData;

namespace DAL
{
    public class UserAccess : IUserAcess
    {

        public IEnumerable<User> LoginUser(string userName, string password)
        {
            var users = ReadUsersData();
            return users;
        }

        public List<User> ReadUsersData()
        {
            var usersList = new List<User>();

            string json;

            using (StreamReader sr = new StreamReader($"JsonData/Users.json"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    usersList = JsonSerializer.Deserialize<List<User>>(json);

                    return usersList;
                }
                return usersList;
            }
        }

        public void UserToFile(User user)
        {
            var list = ReadUsersData();

            list.Add(user);

            PrintToFile(list);
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
