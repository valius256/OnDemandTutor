using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<NotificationGetDto>), 200)]
        public async Task<IActionResult> GetNotifications([FromQuery] PagingModel<NotificationGetDto> pagingModel)
        {
            var notifications = await _notificationService.GetNotificationsAsync(pagingModel);
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotificationGetDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(NotificationGetDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateDto notificationCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdNotification = await _notificationService.CreateNotificationAsync(notificationCreateDto);
            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.Id }, createdNotification);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NotificationGetDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] NotificationGetDto notificationGetDto)
        {
            if (id != notificationGetDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedNotification = await _notificationService.UpdateNotificationAsync(notificationGetDto);
                return Ok(updatedNotification);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                await _notificationService.DeleteNotificationAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
