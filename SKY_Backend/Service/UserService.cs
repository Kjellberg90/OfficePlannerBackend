using DAL;
using DAL.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service

{
    public class UserService : IUserService
    {
        private readonly IUserAcess _userAcess;
        public UserService(IUserAcess userAcess)
        {
            _userAcess = userAcess;
        }

        public SuccessLoginDTO UserLogin(UserLoginDTO login)
        {
			try
			{
                var user = _userAcess.LoginUser(login.UserName, login.Password)
                    .Where(x => x.UserName == login.UserName && x.Password == login.Password)
                    .FirstOrDefault();
                
                if (user != null)
                {
                    return new SuccessLoginDTO() { Id = user.Id, Name = user.UserName, };
                }
                
                return null;
                
			}
			catch (Exception ex)
			{

                throw new Exception(ex.Message);
            }
        }

        public void UserRegister(UserRegisterDTO register)
        {

            var newUser = new User()
            {
                Id = GetUserId(),
                UserName = register.userName,
                Password = register.password
            };
            
            
            _userAcess.UserToFile(newUser);
        }

        public int GetUserId()
        {
            var users = _userAcess.ReadUsersData();
            
            if (users == null)
            {
                return 1;
            }
            
            var lastId = users
                .OrderBy(x => x.Id)
                .LastOrDefault()
                .Id;
            
            return lastId + 1;

        }

        

        
    }
}
