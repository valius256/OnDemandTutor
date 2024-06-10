using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth
{
    public interface IJwtProviderServices
    {
        Task<string> GetForCredentialsAsync(string email, string password);
    }
}
