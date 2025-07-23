using Application_Tenancy.Service;
using Microsoft.AspNetCore.Mvc;
using Application_Tenancy.Models;

namespace Tenancy_Services.Controller
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("Get")]
        public IActionResult Get() => Ok(_service.GetUsers());

        [HttpPost("Post")]
        public IActionResult Post([FromBody] User user)
        {
            _service.AddUser(user);
            return Ok();
        }
    }
}
