using ModelApp.Domain.Dtos;
using ModelApp.Domain.Entities;
using ModelApp.Infra.Repositories.Interfaces;
using ModelApp.Service.Helpers.Interfaces;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICypherHelper _cypherHelper;

        public UserService(IUserRepository userRepository, ICypherHelper cypherHelper)
        {
            _userRepository = userRepository;
            _cypherHelper = cypherHelper;
        }

        public async Task<User> AddAsync(User entity)
        {
            User user = await _userRepository.DetailAsync(entity);

            if (user != null)
                throw new Exception("User already exists.");

            entity.Password = _cypherHelper.Encrypt(entity.Password);

            if (!string.IsNullOrEmpty(entity.SecretPhrase))
                entity.SecretPhrase = _cypherHelper.Encrypt(entity.SecretPhrase);

            return await _userRepository.CreateAsync(entity);
        }

        public async Task<IEnumerable<User>> GetAllAsync(User? entity = null)
        {
            return await _userRepository.RetrieveAsync(entity);
        }

        public async Task<User> GetAsync(int id)
        {
            return await _userRepository.DetailAsync(new User() { Id = id });
        }

        public async Task<User> RefreshAsync(int id, User entity)
        {
            if (entity == null)
                throw new ArgumentNullException("User is null.");

            User user = await _userRepository.DetailAsync(new User() { Id = id });

            if (user == null)
                throw new Exception("The record to be updated was not found.");

            user.UserRoleId = entity.UserRoleId;
            user.Login = user.Login;
            user.Name = entity.Name;
            user.Password = _cypherHelper.Encrypt(entity.Password);
            user.SecretPhrase = entity.SecretPhrase;
            user.Email = entity.Email;
            user.Avatar = entity.Avatar;

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<User> RemoveAsync(int id)
        {
            User customer = await _userRepository.DetailAsync(new User() { Id = id });

            if (customer == null)
                throw new Exception("The record to be deleted was not found.");

            customer.Active = false;

            return await _userRepository.DeleteAsync(customer);
        }

        public async Task<UserDto> GetAuthentication(User user)
        {
            user.Password = _cypherHelper.Encrypt(user.Password);

            return await _userRepository.GetAuthenticate(user);
        }
    }
}
