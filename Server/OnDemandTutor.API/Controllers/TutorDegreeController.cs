using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TutorDegreeController : ControllerBase
{
    private readonly ITutorDegreeService _tutorDegreeService;

    public TutorDegreeController(ITutorDegreeService tutorDegreeService)
    {
        _tutorDegreeService = tutorDegreeService;
    }

    //[Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(PagedResult<GetTutorDegreeDto>), 200)]
    public async Task<IActionResult> GetTutorDegrees([FromQuery] PagingModel<GetTutorDegreeDto> request)
    {
        var tutorDegrees = await _tutorDegreeService.GetTutorDegreesAsync(request);
        return Ok(tutorDegrees);
    }

    //[Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetTutorDegreeDto), 200)]
    public async Task<IActionResult> GetTutorDegreeById(int id)
    {
        var tutorDegree = await _tutorDegreeService.GetTutorDegreeByIdAsync(id);
        if (tutorDegree == null) return NotFound();
        return Ok(tutorDegree);
    }

    //[Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetTutorDegreeDto), 201)]
    public async Task<IActionResult> CreateTutorDegree([FromBody] CreateTutorDegreeDto tutorDegreeDto)
    {
        var createdTutorDegree = await _tutorDegreeService.CreateTutorDegreeAsync(tutorDegreeDto);
        return CreatedAtAction(nameof(GetTutorDegreeById), new { createdTutorDegree.Id }, createdTutorDegree);
    }

    //[Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(UpdateTutorDegreeDto), 200)]
    public async Task<IActionResult> UpdateTutorDegree(int id, [FromBody] UpdateTutorDegreeDto tutorDegreeDto)
    {
        if (id != tutorDegreeDto.Id) return BadRequest("ID mismatch between route parameter and request body.");
        var updatedTutorDegree = await _tutorDegreeService.UpdateTutorDegreeAsync(tutorDegreeDto);
        if (updatedTutorDegree == null) return NotFound();
        return Ok(updatedTutorDegree);
    }

    //[Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteTutorDegree(int id)
    {
        var isDeleted = await _tutorDegreeService.DeleteTutorDegreeAsync(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }
}