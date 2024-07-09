using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.Models.Dtos.FAQ
{
    public class UpdateFAQDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string? Answer { get; set; }
        public string  UpdateBy { get; set; }
        public object MyProperty { get; set; }
    }
}