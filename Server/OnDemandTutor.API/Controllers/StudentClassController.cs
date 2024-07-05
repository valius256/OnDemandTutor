using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentClassController : ControllerBase
    {
        private readonly IStudentClassService _studentClassService;

        public StudentClassController(IStudentClassService studentClassService)
        {
            _studentClassService = studentClassService;
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(CreateStudentClassDto), 200)]
        public async Task<ActionResult> CreateStudentClass([FromBody] CreateStudentClassDto studentClassDto)
        {
            var result = await _studentClassService.CreateStudentClassAsync(studentClassDto);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(PagedResult<GetStudentClassDto>), 200)]
        public async Task<ActionResult<PagedResult<GetStudentClassDto>>> GetStudentClasses([FromQuery] PagingModel<GetStudentClassDto> pagingModel)
        {
            var result = await _studentClassService.GetStudentClassesAsync(pagingModel);
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetStudentClassDto), 200)]
        public async Task<ActionResult<GetStudentClassDto>> GetStudentClassById(int id)
        {
            var result = await _studentClassService.GetStudentClassByIdAsync(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(UpdateStudentClassDto), 200)]
        public async Task<ActionResult<UpdateStudentClassDto>> UpdateStudentClass([FromBody] UpdateStudentClassDto studentClassDto)
        {
            var result = await _studentClassService.UpdateStudentClassAsync(studentClassDto);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<ActionResult<bool>> DeleteStudentClass(int id)
        {
            var result = await _studentClassService.DeleteStudentClassAsync(id);
            return Ok(result);
        }
    }
}
