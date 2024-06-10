using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.DataAccess.ExceptionModels
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
