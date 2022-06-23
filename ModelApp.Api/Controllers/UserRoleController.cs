using Microsoft.AspNetCore.Mvc;
using ModelApp.Domain.Entities;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService customerService)
        {
            _userRoleService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> Get()
        {
            try
            {
                return Ok(await _userRoleService.GetAllAsync());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> Get(int id)
        {
            try
            {
                return Ok(await _userRoleService.GetAsync(new UserRole() { Id = id }));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Post(UserRole userRole)
        {
            try
            {
                return Ok(await _userRoleService.AddAsync(userRole));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> Put(int id, UserRole userRole)
        {
            try
            {
                return Ok(await _userRoleService.RefreshAsync(id, userRole));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            try
            {
                return Ok(await _userRoleService.RemoveAsync(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
