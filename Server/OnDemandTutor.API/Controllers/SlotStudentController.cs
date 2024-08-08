using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Dtos.StudentSlot;
using OnDemandTutor.Models.Paging;

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

        [HttpGet("get-student-slots-tutor")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(PagingModel<GetSlotStudentDetailDto>), 200)]
        public async Task<IActionResult> QuerySlotStudentOfTutor([FromQuery] PagingModel<QueryRatingDto> querySlotStudentDto)
        {
            var slotStudent = await _slotStudentService.GetStudentSlotByTutor(querySlotStudentDto);
            return Ok(slotStudent);
        }

        [Authorize]
        [HttpGet("get-upcoming-slot")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetSlotStudentDetailDto), 200)]
        public async Task<IActionResult> GetUpcomingSlot()
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var slotStudent = await _slotStudentService.GetClosestFutureSlot(user);
            return Ok(slotStudent);
        }
        //[Authorize]
        [HttpGet("{slotId}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(IEnumerable<SlotStudentDto>), 200)]
        public async Task<IActionResult> GetStudentSlotsOfSlot([FromRoute] int slotId, [FromQuery] int page, [FromQuery] int limit)
        {
            var slotStudents = await _slotStudentService.GetSlotStudentsOfSlotPaged(slotId, page, limit);
            return Ok(slotStudents);
        }
        //[Authorize]
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

       
        //[Authorize]
        //[HttpPost("{slotId}/{studentId}/pay")]
        //[ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        //[ProducesResponseType(204)]
        //public async Task<IActionResult> SlotStudentPaid(int slotId, int studentId)
        //{
        //    var result = await _slotStudentService.SlotStudentPaidAsync(slotId, studentId);

        //    if (result)
        //    {
        //        return NoContent();
        //    }

        //    // If SlotStudentPaidAsync fails in a way other than throwing an exception
        //    // handle it by returning a BadRequest with a generic message.
        //    return BadRequest("Payment failed.");
        //}

        [Authorize]
        [HttpPut("feedback-rating")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSlotStudent([FromQuery] int slotId, [FromBody] UpdateSlotStudentDto updateDto)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var result = await _slotStudentService.UpdateSlotStudentAsync(slotId, user.Id, updateDto.Rate, updateDto.Feedback);

            if (result)
            {
                return NoContent();
            }

            // This line will never be reached if the method handles all cases correctly.
            return BadRequest("Update failed.");
        }
    }
}
