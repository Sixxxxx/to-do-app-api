using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Dtos.Response;
using ToDoList.Services.Infrastructure;
using ToDoList.Services.Interfaces;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Authorization")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("", Name = "Create-New-User")]
        [SwaggerOperation(Summary = "Creates user")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "UserId of created user", Type = typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "User with provided email already exists", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create user", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateUser(UserRegistrationRequest request)
        {
            string response = await _authService.CreateUser(request);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("login", Name = "Login")]
        [SwaggerOperation(Summary = "Authenticates user")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "returns user Id and sends token to user's mail after successful authentication", Type = typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
        {
            AuthenticationResponse response = await _authService.UserLogin(request);
            return Ok(response);
        }
    }
}
