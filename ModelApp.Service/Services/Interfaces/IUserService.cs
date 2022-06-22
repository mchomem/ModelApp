using ModelApp.Domain.Dtos;
using ModelApp.Domain.Entities;

namespace ModelApp.Service.Services.Interfaces
{
    public interface IUserService : IService<User>
    {
        public Task<UserDto> GetAuthentication(User user);
    }
}
