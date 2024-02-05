using FoodOrderSystemAPI.DTOs;
using FoodOrderSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderSystemAPI.Controllers
{
    [Route("api/Menus")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly MenuService _service;

        public MenusController(MenuService service)
        {
            _service = service;
        }


        // GET: api/Menus
        [HttpGet, Authorize]
        public async Task<ActionResult<List<MenuDTO>>> GetMenus()
        {
            return await _service.GetMenus();
        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDTO>> GetMenu(int id)
        {
            return await _service.GetMenu(id);
        }

        // POST: api/Menus
        [HttpPost]
        public async Task<ActionResult<MenuDTO>> PostMenu(MenuDTO menuDTO)
        {
            await _service.CreateMenu(menuDTO);
            return CreatedAtAction(nameof(GetMenu), new { id = menuDTO.MenuId }, menuDTO);
        }

        // PUT: api/Menus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, MenuDTO menuDTO)
        {
            return await _service.PutMenu(id, menuDTO);
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            return await _service.DeleteMenu(id);
        }

    }
}
