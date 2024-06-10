using FirebaseAdmin.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.Models.Dtos.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.BusinessLogic.Services.Auth
{
    public class FirebaseAuthServices : IFireBaseAuthServices
    {
        public async Task<string> RegisterUser(RegisterDtos registerDtos)
        {
            var userForFireBaseAuth = new UserRecordArgs()
            {
                Email = registerDtos.Email,
                Password = registerDtos.Password
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userForFireBaseAuth);
            return userRecord.Uid;
        }
        

    }
}
