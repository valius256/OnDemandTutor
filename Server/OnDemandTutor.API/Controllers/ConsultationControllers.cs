using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class ConsultationControllers : BaseController<ConsultationControllers>
    {
        private readonly IConsultationRequestService _consultationRequestService;

        public ConsultationControllers(ILogger<ConsultationControllers> logger, IConsultationRequestService consultationRequestService) : base(logger)
        {
            _consultationRequestService = consultationRequestService;
        }

        // [AllowAnonymous]
        [Authorize]
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetConsultationRequestDtos), 200)]
        public async Task<ActionResult> RegisterForConsultation([FromBody] GetConsultationRequestDtos requestDtos)
        {
            return Ok(await _consultationRequestService.CreateConsultationRequestAsync(requestDtos));
        }

        

        // [AllowAnonymous]
        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(IApiResult<PagedResult<GetConsultationRequestDtos>>), 200)]
        public async Task<IApiResult<PagedResult<GetConsultationRequestDtos>>> GetAllConsultationRequest(PagingModel<GetConsultationRequestDtos> requestDtos)
        {
            return OKAsync(await _consultationRequestService.GetConsultationRequestsAsync(requestDtos));
        }
        
        // [Authorize]
        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(IApiResult<GetConsultationRequestDtos>), 200)]
        public async Task<IApiResult<GetConsultationRequestDtos>> GetAllConsultationRequest([FromBody] int id)
        {
            return OKAsync(await _consultationRequestService.GetConsultationRequestByIdAsync(id));
        }

        // [AllowAnonymous]
        [HttpPost("Handle")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IApiResult<bool>> HandleConsultationRequest([FromBody] HandleConsultationRequestDtos requestDtos)
        {
            return OKAsync(await _consultationRequestService.HandleConsultationRequestAsync(HttpContext.User, requestDtos));
        }
        
    }
}
