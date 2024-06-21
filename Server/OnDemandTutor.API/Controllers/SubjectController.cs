using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.RequestModel.Subject;

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

    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    [HttpGet("exists/{subjectName}")]
    public async Task<IActionResult> CheckSubjectExists(string subjectName)
    {
        var exists = await _subjectService.CheckSubjectExists(subjectName);
        return Ok(exists);
    }

    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetSubjectByCode(int code)
    {
        var subject = await _subjectService.GetSubjectByCode(code);
        if (subject == null)
            return NotFound();
        return Ok(subject);
    }


    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetSubjectByName(string name)
    {
        var subject = await _subjectService.GetSubjectByName(name);
        if (subject == null)
            return NotFound();
        return Ok(subject);
    }


    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetSubjectsByCategory(string category)
    {
        var subjects = await _subjectService.GetSubjectsByCategory(category);
        return Ok(subjects);
    }

    [Authorize]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    [HttpGet("active/{subjectId}")]
    public async Task<IActionResult> IsSubjectActive(int subjectId)
    {
        var isActive = await _subjectService.IsSubjectActive(subjectId);
        return Ok(isActive);
    }

    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchSubjectsByName(string name)
    {
        var subjects = await _subjectService.SearchSubjectsByName(name);
        return Ok(subjects);
    }

    [Authorize]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateSubjectDescription([FromBody] SubjectRequestModel request)
    {
        await _subjectService.UpdateSubjectDescription(request);
        return NoContent();
    }
}