using ModelApp.Domain.Entities;
using ModelApp.Infra.Repositories.Interfaces;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.Service.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository customerRepository)
        {
            _userRoleRepository = customerRepository;
        }

        public async Task<UserRole> AddAsync(UserRole entity)
        {
            UserRole userRole = await _userRoleRepository.DetailAsync(entity);

            if (userRole != null)
                throw new Exception("UserRole already exists.");

            return await _userRoleRepository.CreateAsync(entity);
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync(UserRole? entity = null)
        {
            return await _userRoleRepository.RetrieveAsync(entity);
        }

        public async Task<UserRole> GetAsync(int id)
        {
            return await _userRoleRepository.DetailAsync(new UserRole() { Id = id });
        }

        public async Task<UserRole> RefreshAsync(int id, UserRole entity)
        {
            if (entity == null)
                throw new ArgumentNullException("UserRole is null.");

            UserRole userRole = await _userRoleRepository.DetailAsync(new UserRole() { Id = id });

            if (userRole == null)
                throw new Exception("The record to be updated was not found.");

            userRole.Name = entity.Name;
            userRole.Description = entity.Description;

            return await _userRoleRepository.UpdateAsync(userRole);
        }

        public async Task<UserRole> RemoveAsync(int id)
        {
            UserRole userRole = await _userRoleRepository.DetailAsync(new UserRole() { Id = id });

            if (userRole == null)
                throw new Exception("The record to be deleted was not found.");

            return await _userRoleRepository.DeleteAsync(userRole);
        }
    }
}
