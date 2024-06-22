using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces.Subject;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;


namespace OnDemandTutor.API.Controllers;

[Route("api/subject")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService userService)
    {
        _subjectService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GetSubjectDtos>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetSubjects([FromQuery] PagingModel<GetSubjectDtos> pagingModel)
    {
        var result = await _subjectService.GetSubjects(pagingModel);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetSubjectDtos), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        var result = await _subjectService.GetSubjectById(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetSubjectDtos), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDtos createSubjectDto)
    {
        var result = await _subjectService.CreateSubject(createSubjectDto);
        return CreatedAtAction(nameof(GetSubjectById), new { id = result.id }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectDtos updateSubjectDto)
    {
        if (id != updateSubjectDto.Id)
        {
            return BadRequest();
        }

        var existingSubject = await _subjectService.GetSubjectById(id);
        if (existingSubject == null)
        {
            return NotFound();
        }

        await _subjectService.UpdateSubject(updateSubjectDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var existingSubject = await _subjectService.GetSubjectById(id);
        if (existingSubject == null)
        {
            return NotFound();
        }

        await _subjectService.DeleteSubject(id);
        return NoContent();
    }
}