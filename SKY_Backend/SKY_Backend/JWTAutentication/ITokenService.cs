using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITokenService
    {
        public string CreateJWTToken(SuccessLoginDTO result);
    }
}
