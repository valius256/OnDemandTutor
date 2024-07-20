using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SlotController : ControllerBase
{
    private readonly ISlotServices _slotService;

    public SlotController(ISlotServices slotService)
    {
        _slotService = slotService;
    }
    //[Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GetSlotsDtos>), 200)]
    public async Task<IActionResult> GetSlots([FromQuery] PagingModel<QuerySlotDto> pagingModel)
    {

        var slots = await _slotService.GetSlotsAsync(pagingModel);
        return Ok(slots);

    }
    [HttpGet("tutor-slot-student")]
    [ProducesResponseType(typeof(PagedResult<GetSlotWithSlotStudentWithStudentDetailDto>), 200)]
    public async Task<IActionResult> GetSlotWithStudentOfTutor([FromQuery] int tutorId, [FromQuery] int page = 1, [FromQuery] int limit = 10)
    {

        var slots = await _slotService.GetSlotWithStudentOfTutors(tutorId,page,limit);
        return Ok(slots);

    }
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetSlotDetailDto), 200)]
    public async Task<IActionResult> GetSlotById(int id)
    {
        var slot = await _slotService.GetSlotByIdAsync(id);
        if (slot == null)
        {
            return NotFound();
        }
        return Ok(slot);

    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(GetSlotsDtos), 200)]
    public async Task<IActionResult> CreateSlot([FromBody] CreateSlotsDto slotDto)
    {
        var createdSlot = await _slotService.CreateSlotAsync(slotDto);
        return CreatedAtAction(nameof(GetSlotById), new { id = createdSlot.Id }, createdSlot);

    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateSlotDto), 200)]
    public async Task<IActionResult> UpdateSlot(int id, [FromBody] UpdateSlotDto slotDto)
    {
        if (id != slotDto.Id)
        {
            return BadRequest("ID mismatch between route parameter and request body.");
        }
        var updatedSlot = await _slotService.UpdateSlotAsync(slotDto);
        if (updatedSlot == null)
        {
            return NotFound();
        }
        return NoContent();

    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteSlot(int id)
    {
        var isDeleted = await _slotService.DeleteSlotAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();

    }

    [Authorize]
    [HttpPost("enroll-slot")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> EnrollSlot([FromBody] EnrollSlotDto request)
    {
        var result = await _slotService.EnrollForSlot(request.studentId, request.slotId);
        return Ok(result);
    }

}