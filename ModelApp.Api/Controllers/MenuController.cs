using Microsoft.AspNetCore.Mvc;
using ModelApp.Domain.Entities;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> Get()
        {
            try
            {
                return Ok(await _menuService.GetAllAsync());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> Get(int id)
        {
            try
            {
                return Ok(await _menuService.GetAsync(new Menu() { Id = id }));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Menu>> Post(Menu menu)
        {
            try
            {
                return Ok(await _menuService.AddAsync(menu));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Menu>> Put(int id, Menu menu)
        {
            try
            {
                return Ok(await _menuService.RefreshAsync(id, menu));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Menu>> Delete(int id)
        {
            try
            {
                return Ok(await _menuService.RemoveAsync(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
