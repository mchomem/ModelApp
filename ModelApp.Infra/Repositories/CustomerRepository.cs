using Microsoft.EntityFrameworkCore;
using ModelApp.Domain.Entities;
using ModelApp.Infra.Contexts;
using ModelApp.Infra.Repositories.Interfaces;

namespace ModelApp.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ModelAppContext _modelAppContext;

        public CustomerRepository(ModelAppContext modelAppContext)
        {
            _modelAppContext = modelAppContext;
        }

        public async Task<Customer> CreateAsync(Customer entity)
        {
            _modelAppContext.Add(entity);
            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<Customer> DeleteAsync(Customer entity)
        {
            _modelAppContext.Update(entity);
            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }

        public async Task<Customer> DetailAsync(Customer entity)
        {
            return await _modelAppContext.Customer
                .Where(x =>
                (
                    (!entity.Id.HasValue || x.Id.Value == entity.Id.Value)
                    && (string.IsNullOrEmpty(entity.Name) || x.Name == entity.Name)
                ))
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
        }

        public async Task<IEnumerable<Customer>> RetrieveAsync(Customer? entity)
        {
            if (entity != null)
                return await _modelAppContext.Customer
                    .Where(x =>
                    (
                        (!entity.Id.HasValue || x.Id.Value == entity.Id.Value)
                        && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                        && (!entity.DateBirth.HasValue || x.DateBirth.Value == entity.DateBirth.Value)
                        && (string.IsNullOrEmpty(entity.PhoneNumber) || x.Name.Contains(entity.PhoneNumber))
                        && (string.IsNullOrEmpty(entity.Address) || x.Name.Contains(entity.Address))
                        && (!entity.Active.HasValue || x.Active.Value == entity.Active.Value)
                    )).ToListAsync();
            else
                return await _modelAppContext.Customer
                    .ToListAsync();
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            _modelAppContext.Update(entity);
            await _modelAppContext.SaveChangesAsync();
            return _modelAppContext.Entry(entity).Entity;
        }
    }
}
