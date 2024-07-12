using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
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
        private readonly IAuthServices _authServices;

        public NotificationController(INotificationService notificationService, IAuthServices authServices)
        {
            _notificationService = notificationService;
            _authServices = authServices;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<NotificationGetDto>), 200)]
        public async Task<IActionResult> GetNotifications([FromQuery] int page = 0, [FromQuery] int limit = 20)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User); 
            var notifications = await _notificationService.GetNotificationsAsync(page, limit, user);
            return Ok(notifications);
        }

        [Authorize]
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
        public async Task<IActionResult> UpdateNotification(int id)
        {
            var updatedNotification = await _notificationService.UpdateViewStatus(id);
            return Ok(updatedNotification);
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
