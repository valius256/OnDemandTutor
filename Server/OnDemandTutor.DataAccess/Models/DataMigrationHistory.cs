using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.DataAccess.Models
{
    public class DataMigrationsHistory : BaseEntityEmpty
    {
        public string MigrationId { get; set; }
        public string ProductVersion { get; set; }
    }
}
