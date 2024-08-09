using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.TutorSubject;
using OnDemandTutor.BusinessLogic.Services.Auth;
using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorSubjectController : ControllerBase
    {
        private readonly ITutorSubjectService _tutorSubjectService;
        private readonly IAuthServices _authServices;

        public TutorSubjectController(ITutorSubjectService tutorSubjectService, IAuthServices authServices)
        {
            _tutorSubjectService = tutorSubjectService;
            _authServices = authServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<GetTutorSubjectWithUserAndSubjectDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> GetTutorSubjects([FromQuery] PagingModel<QueryTutorSubjectDto> pagingModel)
        {
            var tutorSubjects = await _tutorSubjectService.GetTutorSubjectsAsync(pagingModel);
            return Ok(tutorSubjects);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTutorSubjectDetailDto), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> GetTutorSubjectById(int id)
        {
            var tutorSubject = await _tutorSubjectService.GetTutorSubjectByIdAsync(id);
            if (tutorSubject == null)
            {
                return NotFound();
            }
            return Ok(tutorSubject);
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(GetTutorSubjectDetailDto), 201)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> CreateTutorSubject([FromBody] CreateTutorSubjectDto tutorSubjectDto)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var createdTutorSubject = await _tutorSubjectService.CreateTutorSubjectAsync(tutorSubjectDto, user);
            return CreatedAtAction(nameof(GetTutorSubjectById), new {Id = createdTutorSubject.Id} , createdTutorSubject);
        }
        [Authorize]
        [HttpPut("status")]
        [ProducesResponseType(typeof(UpdateTutorSubjectStatusDto), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> UpdateTutorSubject([FromBody] UpdateTutorSubjectStatusDto tutorSubjectDto)
        {
            await _tutorSubjectService.UpdateTutorSubjectStatusAsync(tutorSubjectDto);
            return NoContent();
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateTutorSubjectStatusDto), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> UpdateTutorSubjectStatus([FromBody] UpdateTutorSubjectDto tutorSubjectDto)
        {
            await _tutorSubjectService.UpdateTutorSubjectAsync(tutorSubjectDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> DeleteTutorSubject(int id)
        {
            var isDeleted = await _tutorSubjectService.DeleteTutorSubjectAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
