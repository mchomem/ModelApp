using ModelApp.Domain.Entities;
using ModelApp.Infra.Repositories.Interfaces;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.Service.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<Menu> AddAsync(Menu entity)
        {
            Menu menu = await _menuRepository.DetailAsync(entity);

            if (menu != null)
                throw new Exception("Menu already exists.");

            return await _menuRepository.CreateAsync(entity);
        }

        public async Task<IEnumerable<Menu>> GetAllAsync(Menu? entity = null)
        {
            return await _menuRepository.RetrieveAsync(entity);
        }

        public async Task<Menu> GetAsync(int id)
        {
            return await _menuRepository.DetailAsync(new Menu() { Id = id });
        }

        public async Task<Menu> RefreshAsync(int id, Menu entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Menu is null.");

            Menu menu = await _menuRepository.DetailAsync(new Menu() { Id = id });

            if (menu == null)
                throw new Exception("The record to be updated was not found.");

            menu.ParentMenuId = entity.ParentMenuId;
            menu.Label = entity.Label;
            menu.Page = entity.Page;
            menu.ImageIcon = entity.ImageIcon;
            menu.CssFontAwesomeIcon = entity.CssFontAwesomeIcon;
            menu.Visible = entity.Visible;
            menu.Order = entity.Order;

            return await _menuRepository.UpdateAsync(menu);
        }

        public async Task<Menu> RemoveAsync(int id)
        {
            Menu menu = await _menuRepository.DetailAsync(new Menu() { Id = id });

            if (menu == null)
                throw new Exception("The record to be deleted was not found.");

            menu.Active = false;

            return await _menuRepository.DeleteAsync(menu);
        }
    }
}
