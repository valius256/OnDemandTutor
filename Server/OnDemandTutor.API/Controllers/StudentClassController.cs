using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
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
        private readonly IAuthServices _authServices;

        public StudentClassController(IStudentClassService studentClassService, IAuthServices authServices)
        {
            _studentClassService = studentClassService;
            _authServices = authServices;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(PagedResult<GetStudentClassDetailDto>), 200)]
        public async Task<ActionResult<PagedResult<GetStudentClassDetailDto>>> GetQueriedStudentClass([FromQuery] PagingModel<QueryStudentClassDto> getStudentClassDetailDto)
        {
            var result = await _studentClassService.QueryStudentClassAsync(getStudentClassDetailDto);
            return Ok(result);
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
        [HttpPut("{classId}/leave")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(UpdateStudentClassDto), 200)]
        public async Task<ActionResult<UpdateStudentClassDto>> ActivelyLeaveClass([FromRoute] int classId)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            await _studentClassService.ActivelyLeaveClass(classId, user);
            return NoContent();
        }
    }
}
