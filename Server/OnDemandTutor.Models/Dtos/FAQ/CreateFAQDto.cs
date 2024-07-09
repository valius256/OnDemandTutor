using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.Models.Dtos.FAQ
{
    public class CreateFAQDto
    {
        public string Question { get; set; }
        public string? Answer { get; set; }
    }
}