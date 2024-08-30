using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorDegreeController : ControllerBase
    {
        private readonly ITutorDegreeService _tutorDegreeService;

        public TutorDegreeController(ITutorDegreeService tutorDegreeService)
        {
            _tutorDegreeService = tutorDegreeService;
        }

       
    }
}
