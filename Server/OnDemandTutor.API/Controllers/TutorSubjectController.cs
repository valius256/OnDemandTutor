using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.TutorSubject;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorSubjectController : ControllerBase
    {
        private readonly ITutorSubjectService _tutorSubjectService;

        public TutorSubjectController(ITutorSubjectService tutorSubjectService)
        {
            _tutorSubjectService = tutorSubjectService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<GetTutorSubjectDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> GetTutorSubjects([FromQuery] PagingModel<GetTutorSubjectDto> pagingModel)
        {
            var tutorSubjects = await _tutorSubjectService.GetTutorSubjectsAsync(pagingModel);
            return Ok(tutorSubjects);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTutorSubjectDto), 200)]
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

        [HttpPost]
        [ProducesResponseType(typeof(CreateTutorSubjectDto), 201)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> CreateTutorSubject([FromBody] CreateTutorSubjectDto tutorSubjectDto)
        {
            var createdTutorSubject = await _tutorSubjectService.CreateTutorSubjectAsync(tutorSubjectDto);
            return CreatedAtAction(nameof(GetTutorSubjectById), createdTutorSubject);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateTutorSubjectDto), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> UpdateTutorSubject(int id, [FromBody] UpdateTutorSubjectDto tutorSubjectDto)
        {
            if (id != tutorSubjectDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }
            var updatedTutorSubject = await _tutorSubjectService.UpdateTutorSubjectAsync(tutorSubjectDto);
            if (updatedTutorSubject == null)
            {
                return NotFound();
            }
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
