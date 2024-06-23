using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
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
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(PagedResult<GetSlotsDtos>), 200)]
    public async Task<IActionResult> GetSlots([FromQuery] PagingModel<GetSlotsDtos> pagingModel)
    {
      
            var slots = await _slotService.GetSlotsAsync(pagingModel);
            return Ok(slots);
        
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetSlotsDtos), 200)]
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
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(CreateSlotsDtos), 200)]
    public async Task<IActionResult> CreateSlot([FromBody] CreateSlotsDtos slotDto)
    {
          var createdSlot = await _slotService.CreateSlotAsync(slotDto);
            return CreatedAtAction(nameof(GetSlotById), new { id = createdSlot.Id }, createdSlot);
        
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(UpdateSlotDtos), 200)]
    public async Task<IActionResult> UpdateSlot(int id, [FromBody] UpdateSlotDtos slotDto)
    {
         if (id != slotDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }
            var updatedSlot = await _slotService.UpdateSlotAsync( slotDto);
            if (updatedSlot == null)
            {
                return NotFound();
            }
            return NoContent();
      
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
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
}