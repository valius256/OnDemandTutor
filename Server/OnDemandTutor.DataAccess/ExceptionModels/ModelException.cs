using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.DataAccess.ExceptionModels
{
   public class ModelException : Exception
    {
        public string Field { get; set; }
        public string[] Paras { get; set; }

        public ModelException(string field, string errorCode, params string[] paras)
            : base(errorCode)
        {
            this.Field = field;
            this.Paras = paras;
        }
    }
}
