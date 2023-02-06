using DAL;
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

        public SuccessLoginDTO UserLogin(LoginDTO login)
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
    }
}
