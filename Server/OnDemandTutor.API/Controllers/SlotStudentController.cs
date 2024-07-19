using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotStudentController : ControllerBase
    {
        private readonly ISlotStudentServices _slotStudentService;
        private readonly IAuthServices _authServices;

        public SlotStudentController(ISlotStudentServices slotStudentService, IAuthServices authServices)
        {
            _authServices = authServices;
            _slotStudentService = slotStudentService;
        }

        [Authorize]
        [HttpGet("get-slots-of-students")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(List<GetSlotStudentDetailDto>), 200)]
        public async Task<IActionResult> QuerySlotStudent([FromQuery] QuerySlotStudentDto querySlotStudentDto)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var slotStudent = await _slotStudentService.QuerySlotStudent(querySlotStudentDto, user);
            return Ok(slotStudent);
        }

        [Authorize]
        [HttpGet("get-upcoming-slot")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(List<GetSlotStudentDetailDto>), 200)]
        public async Task<IActionResult> GetUpcomingSlot([FromQuery] QuerySlotStudentDto querySlotStudentDto)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var slotStudent = await _slotStudentService.GetClosestFutureSlot(user);
            return Ok(slotStudent);
        }

        [Authorize]
        [HttpGet("{slotId}/{studentId}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(SlotStudentDto), 200)]
        public async Task<IActionResult> GetSlotStudent(int slotId, int studentId)
        {
            var slotStudent = await _slotStudentService.GetSlotStudentAsync(slotId, studentId);
            if (slotStudent == null)
            {
                return NotFound();
            }
            return Ok(slotStudent);
        }

    }
}
