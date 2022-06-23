using Microsoft.EntityFrameworkCore;
using ModelApp.Domain.Entities;
using ModelApp.Infra.Contexts;
using ModelApp.Infra.Repositories.Interfaces;

namespace ModelApp.Infra.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ModelAppContext _modelAppContext;

        public MenuRepository(ModelAppContext modelAppContext)
        {
            _modelAppContext = modelAppContext;
        }

        public async Task<Menu> CreateAsync(Menu entity)
        {
            _modelAppContext.Add(entity);

            entity.ParentMenu = await this.DetailAsync(new Menu() { Id = entity.ParentMenuId });
            if (entity.ParentMenu == null)
                throw new Exception("ParentMenu not found");

            _modelAppContext.Menu.Attach(entity.ParentMenu);
            _modelAppContext.Entry(entity.ParentMenu).State = EntityState.Unchanged;

            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<Menu> DeleteAsync(Menu entity)
        {
            _modelAppContext.Remove(entity);

            entity.ParentMenu = await this.DetailAsync(new Menu() { Id = entity.ParentMenuId });
            if (entity.ParentMenu == null)
                throw new Exception("ParentMenu not found");

            _modelAppContext.Menu.Attach(entity.ParentMenu);
            _modelAppContext.Entry(entity.ParentMenu).State = EntityState.Unchanged;

            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<Menu> DetailAsync(Menu entity)
        {
            return await _modelAppContext.Menu
                    .Include(x => x.ParentMenu)
                    .Where(x =>
                    (
                        (!entity.Id.HasValue || x.Id == entity.Id)
                        && (string.IsNullOrEmpty(entity.Label) || x.Label == entity.Label)
                    ))
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Menu>> RetrieveAsync(Menu? entity)
        {
            if (entity != null)
                return await _modelAppContext.Menu
                    .Include(x => x.ParentMenu)
                    .Where(x =>
                    (
                        (!entity.Id.HasValue || x.Id.Value == entity.Id.Value)
                        && (string.IsNullOrEmpty(entity.Label) || x.Label.Contains(entity.Label))
                        && (string.IsNullOrEmpty(entity.Page) || x.Page.Contains(entity.Page))
                        && (!entity.Active.HasValue || x.Active.Value == entity.Active.Value)
                        && (!entity.Visible.HasValue || x.Visible.Value == entity.Visible.Value)
                        && (!entity.Order.HasValue || x.Order.Value == entity.Order.Value)
                    )).ToListAsync();
            else
                return await _modelAppContext.Menu
                    .ToListAsync();
        }

        public async Task<Menu> UpdateAsync(Menu entity)
        {
            _modelAppContext.Update(entity);

            entity.ParentMenu = await this.DetailAsync(new Menu() { Id = entity.ParentMenuId });
            if (entity.ParentMenu == null)
                throw new Exception("ParentMenu not found");

            _modelAppContext.Menu.Attach(entity.ParentMenu);
            _modelAppContext.Entry(entity.ParentMenu).State = EntityState.Unchanged;

            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }
    }
}
