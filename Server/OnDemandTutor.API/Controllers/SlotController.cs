using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SlotController : ControllerBase
{
    private readonly ISlotServices _slotService;
    private readonly IAuthServices _authServices;

    public SlotController(ISlotServices slotService, IAuthServices authServices)
    {
        _slotService = slotService;
        _authServices = authServices;
    }
    //[Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GetSlotsDtos>), 200)]
    public async Task<IActionResult> GetSlots([FromQuery] PagingModel<QuerySlotDto> pagingModel)
    {

        var slots = await _slotService.GetSlotsAsync(pagingModel);
        return Ok(slots);

    }
    
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
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        var createdSlot = await _slotService.CreateSlotAsync(slotDto, user);
        return CreatedAtAction(nameof(GetSlotById), new { id = createdSlot.Id }, createdSlot);

    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(GetSlotsDtos), 200)]
    public async Task<IActionResult> UpdateSlot([FromBody] UpdateSlotDto slotDto)
    {
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        await _slotService.UpdateSlotAsync(slotDto, user);
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