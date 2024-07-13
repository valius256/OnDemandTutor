using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.Models.Dtos.SlotStudent;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotStudentController : ControllerBase
    {
        private readonly ISlotStudentServices _slotStudentService;

        public SlotStudentController(ISlotStudentServices slotStudentService)
        {
            _slotStudentService = slotStudentService;
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

        [Authorize]
        [HttpPost("{slotId}/{studentId}/pay")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> SlotStudentPaid(int slotId, int studentId)
        {
            try
            {
                var result = await _slotStudentService.SlotStudentPaidAsync(slotId, studentId);
                if (result)
                {
                    return NoContent();
                }
                return BadRequest("Payment failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorActionResult
                {
                    Status = 400,
                    Title = ex.Message
                });
            }
        }
    }
}
