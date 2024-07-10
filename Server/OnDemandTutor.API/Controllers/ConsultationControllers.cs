using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationControllers : BaseController<ConsultationControllers>
    {
        private readonly IConsultationRequestService _consultationRequestService;

        public ConsultationControllers(ILogger<ConsultationControllers> logger, IConsultationRequestService consultationRequestService) : base(logger)
        {
            _consultationRequestService = consultationRequestService;
        }

        // [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetConsultationRequestDto), 200)]
        public async Task<ActionResult> RegisterForConsultation([FromBody] RegisterConsultationRequestDto requestDtos)
        {
            return Ok(await _consultationRequestService.CreateConsultationRequestAsync(requestDtos));
        }



        // [AllowAnonymous]
        [HttpGet("all")]
        // [Authorize]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(IApiResult<PagedResult<GetConsultationRequestDto>>), 200)]
        public async Task<IApiResult<PagedResult<GetConsultationRequestDto>>> GetAllConsultationRequest([FromQuery] ConsultationRequestFilterDto requestDtos)
        {
            return OKAsync(await _consultationRequestService.ViewAllConsultationsRequestAsync(requestDtos));
        }

        // [Authorize]
        [HttpGet("get-by-id")]
        [Authorize]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(IApiResult<GetConsultationRequestDto>), 200)]
        public async Task<IApiResult<GetConsultationRequestDto>> GetAllConsultationRequest(int id)
        {
            return OKAsync(await _consultationRequestService.GetConsultationRequestByIdAsync(id));
        }


        [Authorize]
        [HttpPatch("Handle")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IApiResult<bool>> HandleConsultationRequest([FromBody] HandleConsultationRequestDto requestDto)
        {
            return OKAsync(await _consultationRequestService.HandleConsultationRequestAsync(HttpContext.User, requestDto));
        }

        [HttpDelete("delete")]
        [Authorize]//[Authorize(Roles = "Operator, Admin")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IApiResult<bool>> DeleteConsultationRequest([FromQuery]int id)
        {
            return OKAsync(await _consultationRequestService.DeleteConsultationRequestAsync(id));
        }
    }
}
