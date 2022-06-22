using Microsoft.EntityFrameworkCore;
using ModelApp.Domain.Dtos;
using ModelApp.Domain.Entities;
using ModelApp.Infra.Contexts;
using ModelApp.Infra.Repositories.Interfaces;

namespace ModelApp.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ModelAppContext _modelAppContext;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRepository(ModelAppContext modelAppContext, IUserRoleRepository userRoleRepository)
        {
            _modelAppContext = modelAppContext;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<User> CreateAsync(User entity)
        {
            _modelAppContext.Add(entity);

            UserRole userRole = await _userRoleRepository.DetailAsync(new UserRole() { Id = entity.UserRoleId });
            if (userRole == null)
                throw new Exception("UserRole not found");

            _modelAppContext.UserRole.Attach(userRole);
            _modelAppContext.Entry(entity.UserRole).State = EntityState.Unchanged;

            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<User> DeleteAsync(User entity)
        {
            _modelAppContext.Update(entity);

            UserRole userRole = await _userRoleRepository.DetailAsync(new UserRole() { Id = entity.UserRoleId });
            if (userRole == null)
                throw new Exception("UserRole not found");

            _modelAppContext.UserRole.Attach(userRole);
            _modelAppContext.Entry(entity.UserRole).State = EntityState.Unchanged;

            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<User> DetailAsync(User entity)
        {
            return await _modelAppContext.User
                .Include(x => x.UserRole)
                .Where(x =>
                (
                    (!entity.Id.HasValue || x.Id.Value == entity.Id.Value)
                    && (string.IsNullOrEmpty(entity.Name) || x.Name == entity.Name)
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> RetrieveAsync(User? entity)
        {
            if (entity != null)
                return await _modelAppContext.User
                    .Include(x => x.UserRole)
                    .Where(x =>
                    (
                        (!entity.Id.HasValue || x.Id.Value == entity.Id.Value)
                        && (string.IsNullOrEmpty(entity.Login) || x.Login.Contains(entity.Login))
                        && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                        && (string.IsNullOrEmpty(entity.Email) || x.Name.Contains(entity.Email))
                        && (!entity.Active.HasValue || x.Active.Value == entity.Active.Value)
                    )).ToListAsync();
            else
                return await _modelAppContext.User
                    .ToListAsync();
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _modelAppContext.Update(entity);

            UserRole userRole = await _userRoleRepository.DetailAsync(new UserRole() { Id = entity.UserRoleId });
            if (userRole == null)
                throw new Exception("UserRole not found");

            _modelAppContext.UserRole.Attach(userRole);
            _modelAppContext.Entry(entity.UserRole).State = EntityState.Unchanged;

            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<UserDto> GetAuthenticate(User entity)
        {
            return await _modelAppContext.User
                .Include(x => x.UserRole)
                .Where(x => x.Login == entity.Login
                      && x.Password == entity.Password
                      && x.Active == true)
                .Select(x => new UserDto()
                {
                    Id = x.Id,
                    UserRole = x.UserRole.Name,
                    Name = x.Name,
                    Email = x.Email,
                    Active = x.Active,
                    Avatar = x.Avatar
                })
                .FirstOrDefaultAsync();
        }

        public Task ChangePassword(User entity, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
