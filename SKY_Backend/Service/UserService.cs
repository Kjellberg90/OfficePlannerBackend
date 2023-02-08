using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

                var user = _userAcess.LoginUser(login.UserName)
                    .Where(x => x.UserName == login.UserName)
                    .FirstOrDefault();

                if (user != null)
                {
                    var passwordMatch = PasswordDecryption(login.Password, user.Password);
                    if (passwordMatch)
                    {
                        return new SuccessLoginDTO() { Id = user.Id, Name = user.UserName, };
                    }
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
                Password = PasswordEncryption(register.password)
            };

            var userExists = CheckIfUserExists(register.userName);

            if(!userExists)
            {
                _userAcess.UserToFile(newUser);
            }
        }

        public bool CheckIfUserExists(string userName)
        {
            var users = _userAcess.ReadUsersData();

            var user = users.Any(x => x.UserName == userName);

                if (user)
                {
                    return true;
                } 
            
                return false;
        }

        public int GetUserId()
        {
            var users = _userAcess.ReadUsersData();
            
            if (users.Count == 0)
            {
                return 1;
            }
            
            var lastId = users
                .OrderBy(x => x.Id)
                .LastOrDefault()
                .Id;
            
            return lastId + 1;
        }


        private string PasswordEncryption(string password)
        {
            byte[] salt = new byte[128 / 8];

            using var rng = RandomNumberGenerator.Create();

            rng.GetNonZeroBytes(salt);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            hashedPassword = hashedPassword + "@" + Convert.ToBase64String(salt);

            return hashedPassword;
        }

        private bool PasswordDecryption(string userEnteredPassword, string dbPasswordHash)
        {
            string salt = dbPasswordHash.Split('@')[1];
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userEnteredPassword,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return dbPasswordHash == (hashedPassword + "@" + salt);
        }
    }
}