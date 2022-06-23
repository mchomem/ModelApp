using Microsoft.EntityFrameworkCore;
using ModelApp.Domain.Entities;
using ModelApp.Infra.Contexts;
using ModelApp.Infra.Repositories.Interfaces;

namespace ModelApp.Infra.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ModelAppContext _modelAppContext;

        public UserRoleRepository(ModelAppContext modelAppContext)
        {
            _modelAppContext = modelAppContext;
        }

        public async Task<UserRole> CreateAsync(UserRole entity)
        {
            _modelAppContext.Add(entity);
            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<UserRole> DeleteAsync(UserRole entity)
        {
            _modelAppContext.Remove(entity);
            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<UserRole> DetailAsync(UserRole entity)
        {
            return await _modelAppContext.UserRole
                .Where(x =>
                (
                    (!entity.Id.HasValue || x.Id.Value == entity.Id.Value)
                    && (string.IsNullOrEmpty(entity.Name) || x.Name == entity.Name)
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserRole>> RetrieveAsync(UserRole? entity)
        {
            if (entity != null)
                return await _modelAppContext.UserRole
                    .Where(x =>
                    (
                        (!entity.Id.HasValue || x.Id.Value == entity.Id.Value)
                        && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                        && (string.IsNullOrEmpty(entity.Description) || x.Name.Contains(entity.Description))
                    )).ToListAsync();
            else
                return await _modelAppContext.UserRole
                    .ToListAsync();
        }

        public async Task<UserRole> UpdateAsync(UserRole entity)
        {
            _modelAppContext.Update(entity);
            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }
    }
}
