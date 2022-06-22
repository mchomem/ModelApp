using ModelApp.Domain.Dtos;
using ModelApp.Domain.Entities;

namespace ModelApp.Infra.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<UserDto> GetAuthenticate(User entity);

        public Task ChangePassword(User entity, string newPassword);
    }
}
