using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth
{
    public interface IFireBaseAuthServices
    {
        Task<string> RegisterUser(RegisterDtos registerDtos);

    }
}
