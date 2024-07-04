using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Subject;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<GetSubjectDtos>), 200)]
        public async Task<IActionResult> GetSubjects([FromQuery] PagingModel<GetSubjectDtos> pagingModel)
        {
            var subjects = await _subjectService.GetSubjectsAsync(pagingModel);
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetSubjectDtos), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateSubjectDtos), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDtos subjectCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSubject = await _subjectService.CreateSubjectAsync(subjectCreateDto);
            return CreatedAtAction(nameof(GetSubjectById), createdSubject);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GetSubjectDtos), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] GetSubjectDtos GetSubjectDtos)
        {
            if (id != GetSubjectDtos.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

                var updatedSubject = await _subjectService.UpdateSubjectAsync(GetSubjectDtos);
                return Ok(updatedSubject);
        }

        //[Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubject(int id)
        {
                await _subjectService.DeleteSubjectAsync(id);
                return NoContent();
        }
    }
}
