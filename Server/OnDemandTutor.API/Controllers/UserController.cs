using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.Models.Dtos;

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

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
        public async Task<List<GetProfileUserDtos>> GetAll()
        {
            return await _userService.GetAllUsers();
        }

    }
}
