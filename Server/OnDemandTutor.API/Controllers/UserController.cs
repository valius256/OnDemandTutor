using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        //[Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
        public async Task<List<GetProfileUserDtos>> GetAll()
        {
            return await _userService.GetAllUsers();
        }

        //[Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
        public async Task<ActionResult<GetProfileUserDtos>> GetProfile([FromBody] int userId)
        {
            return await _userService.GetProfile(userId, null);
        }



        //[Authorize]
        [HttpPost("register-tutor")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetProfileTutorDtos), 200)]
        public async Task<ActionResult<GetProfileTutorDtos>> RegisterTutor([FromBody] RegisterTutorDtos body)
        {
            return await _userService.RegisterTutor(body);
        }
    }
}
