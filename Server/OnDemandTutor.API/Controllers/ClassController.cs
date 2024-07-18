using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IAuthServices _authServices;

        public ClassController(IClassService classService, IAuthServices authServices)
        {
            _classService = classService;
            _authServices = authServices;
        }


        //[Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(PagedResult<GetClassDtos>), 200)]
        public async Task<IActionResult> GetClasses([FromQuery] PagingModel<QueryClassDTO> pagingModel)
        {
            var classes = await _classService.GetClasses(pagingModel);
            return Ok(classes);
        }
        [Authorize]
        [HttpGet("student")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(PagedResult<GetClassDtos>), 200)]
        public async Task<IActionResult> GetClassesOfStudent([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var student = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var classes = await _classService.GetClassesOfStudent(student.Id,page,limit);
            return Ok(classes);
        }
        [Authorize]
        [HttpGet("tutor")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(PagedResult<GetClassDtos>), 200)]
        public async Task<IActionResult> GetClassesOfTutor([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var tutor = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var classes = await _classService.GetClassesOfStudent(tutor.Id, page, limit);
            return Ok(classes);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetClassFullDataSlotDto), 200)]
        public async Task<IActionResult> GetClassById(int id)
        {
            var classDto = await _classService.GetClassByIdAsync(id);
            if (classDto == null)
            {
                return NotFound();
            }
            return Ok(classDto);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetClassDtos), 200)]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassDTO classDto)
        {
            var createdClass = await _classService.CreateClassAsync(classDto);
            return CreatedAtAction(nameof(GetClassById), createdClass);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetClassDtos), 200)]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] GetClassDtos classDto)
        {
            if (id != classDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }
            var updatedClass = await _classService.UpdateClassAsync(classDto);
            if (updatedClass == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var isDeleted = await _classService.DeleteClassAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

