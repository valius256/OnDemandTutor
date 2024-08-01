using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.TutorVideo;
using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorVideoController : ControllerBase
    {
        private readonly ITutorVideoService _tutorVideoService;
        private readonly IAuthServices _authServices;

        public TutorVideoController(ITutorVideoService tutorVideoService, IAuthServices authServices)
        {
            _tutorVideoService = tutorVideoService;
            _authServices = authServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<GetTutorVideoDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> GetTutorVideos([FromQuery] PagingModel<QueryTutorVideoDto> pagingModel)
        {
            var tutorVideos = await _tutorVideoService.GetTutorVideosAsync(pagingModel);
            return Ok(tutorVideos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTutorVideoDto), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> GetTutorVideoById(int id)
        {
            var tutorVideo = await _tutorVideoService.GetTutorVideoByIdAsync(id);
            return Ok(tutorVideo);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateTutorVideoDto), 201)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> CreateTutorVideo([FromBody] CreateTutorVideoDto tutorVideoDto)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var createdTutorVideo = await _tutorVideoService.CreateTutorVideoAsync(tutorVideoDto, user);
            return Ok(createdTutorVideo);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateTutorVideoDto), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> UpdateTutorVideo(int id, [FromBody] UpdateTutorVideoDto tutorVideoDto)
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            await _tutorVideoService.UpdateTutorVideoAsync(tutorVideoDto, user);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> DeleteTutorVideo(int id)
        {
            await _tutorVideoService.DeleteTutorVideoAsync(id);
            return NoContent();
        }
    }
}
