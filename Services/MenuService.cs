using FoodOrderSystemAPI.DTOs;
using FoodOrderSystemAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderSystemAPI.Services
{
    public class MenuService
    {
        private readonly FoodOrderingSystemContext _context;

        public MenuService(FoodOrderingSystemContext context)
        {
            _context = context;
        }

        public async Task<List<MenuDTO>> GetMenus()
        {
            var menus = await (from m in _context.Menus
                               join ms in _context.MenuStatuses on m.MenuStatusId equals ms.MenuStatusId
                               select new MenuDTO
                               {
                                   MenuId = m.MenuId,
                                   MenuStatusId = m.MenuStatusId,
                                   MenuStatus = ms.Name,
                                   Name = m.Name
                               })
                               .AsNoTracking()
                               .ToListAsync();

            return menus;
        }

        public async Task<ActionResult<MenuDTO>> GetMenu(int id)
        {
            var menu = await (from m in _context.Menus
                              join ms in _context.MenuStatuses on m.MenuStatusId equals ms.MenuStatusId
                              where m.MenuId == id
                              select new MenuDTO
                              {
                                  MenuId = m.MenuId,
                                  MenuStatusId = m.MenuStatusId,
                                  MenuStatus = ms.Name,
                                  Name = m.Name
                              })
                               .AsNoTracking()
                               .FirstOrDefaultAsync();

            if (menu == null)
            {
                return new NotFoundResult();
            }

            return menu;
        }

        public async Task CreateMenu(MenuDTO menuDTO)
        {
            var menu = new Menu
            {
                Name = menuDTO.Name,
                MenuStatusId = menuDTO.MenuStatusId
            };

            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult> DeleteMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return new NotFoundResult();
            }

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<ActionResult> PutMenu(int id, MenuDTO menuDTO)
        {
            if (id != menuDTO.MenuId)
            {
                return new BadRequestResult();
            }

            var menuEntity = new Menu
            {
                MenuId = menuDTO.MenuId,
                MenuStatusId = menuDTO.MenuStatusId,
                Name = menuDTO.Name
            };

            _context.Entry(menuEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        private bool MenuExists(int id)
        {
            return (_context.Menus?.Any(e => e.MenuId == id)).GetValueOrDefault();
        }

    }
}
