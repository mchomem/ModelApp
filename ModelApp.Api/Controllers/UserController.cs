using Microsoft.AspNetCore.Mvc;
using ModelApp.Domain.Dtos;
using ModelApp.Domain.Entities;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService customerService)
        {
            _userService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            try
            {
                return Ok(await _userService.GetAllAsync());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                return Ok(await _userService.GetAsync(new User() { Id = id }));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("/authentication")]
        public async Task<ActionResult<UserDto>> GetAuthentication(string login, string password)
        {
            try
            {
                return Ok(await _userService.GetAuthentication(new User() { Login = login, Password = password }));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            try
            {
                return Ok(await _userService.AddAsync(user));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(int id, User user)
        {
            try
            {
                return Ok(await _userService.RefreshAsync(id, user));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            try
            {
                return Ok(await _userService.RemoveAsync(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
