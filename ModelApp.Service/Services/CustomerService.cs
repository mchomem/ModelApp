using ModelApp.Domain.Entities;
using ModelApp.Infra.Repositories.Interfaces;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            Customer customer = await _customerRepository.DetailAsync(entity);

            if (customer != null)
                throw new Exception("Customer already exists.");

            return await _customerRepository.CreateAsync(entity);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(Customer? entity = null)
        {
            return await _customerRepository.RetrieveAsync(entity);
        }

        public async Task<Customer> GetAsync(Customer customer)
        {
            return await _customerRepository.DetailAsync(customer);
        }

        public async Task<Customer> RefreshAsync(int id, Customer entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Customer is null.");

            Customer customer = await _customerRepository.DetailAsync(new Customer() { Id = id });

            if (customer == null)
                throw new Exception("The record to be updated was not found.");

            customer.Name = entity.Name;
            customer.DateBirth = entity.DateBirth;
            customer.PhoneNumber = entity.PhoneNumber;
            customer.Address = entity.Address;

            return await _customerRepository.UpdateAsync(customer);
        }

        public async Task<Customer> RemoveAsync(int id)
        {
            Customer customer = await _customerRepository.DetailAsync(new Customer() { Id = id });

            if (customer == null)
                throw new Exception("The record to be deleted was not found.");

            customer.Active = false;

            return await _customerRepository.DeleteAsync(customer);
        }
    }
}
