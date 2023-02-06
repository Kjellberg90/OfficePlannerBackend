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
        public bool UserLogin(LoginDTO login)
        {
			try
			{
                return true;
                
			}
			catch (Exception ex)
			{

                throw new Exception(ex.Message);
            }
        }
    }
}
