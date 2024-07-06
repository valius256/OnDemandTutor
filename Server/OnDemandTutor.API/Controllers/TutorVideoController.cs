using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.TutorVideo;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorVideoController : ControllerBase
    {
        private readonly ITutorVideoService _tutorVideoService;

        public TutorVideoController(ITutorVideoService tutorVideoService)
        {
            _tutorVideoService = tutorVideoService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<GetTutorVideoDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> GetTutorVideos([FromQuery] PagingModel<GetTutorVideoDto> pagingModel)
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
            if (tutorVideo == null)
            {
                return NotFound();
            }
            return Ok(tutorVideo);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTutorVideoDto), 201)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> CreateTutorVideo([FromBody] CreateTutorVideoDto tutorVideoDto)
        {
            var createdTutorVideo = await _tutorVideoService.CreateTutorVideoAsync(tutorVideoDto);
            return CreatedAtAction(nameof(GetTutorVideoById), createdTutorVideo);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateTutorVideoDto), 200)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> UpdateTutorVideo(int id, [FromBody] UpdateTutorVideoDto tutorVideoDto)
        {
            if (id != tutorVideoDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }
            var updatedTutorVideo = await _tutorVideoService.UpdateTutorVideoAsync(tutorVideoDto);
            if (updatedTutorVideo == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        public async Task<IActionResult> DeleteTutorVideo(int id)
        {
            var isDeleted = await _tutorVideoService.DeleteTutorVideoAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
