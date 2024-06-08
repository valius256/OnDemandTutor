using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.DataAccess.ExceptionModels
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string value) : base(value)
        {
        }
    }
}
