using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserService
{
    public interface IUserService
    {
        public SuccessLoginDTO UserLogin(UserLoginDTO login);
        public void UserRegister(UserRegisterDTO register);
        public void AdminRegister(UserRegisterDTO register);
        public string GetUserRole(SuccessLoginDTO user);
    }
}
