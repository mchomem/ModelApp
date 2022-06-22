namespace ModelApp.Infra.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T> CreateAsync(T entity);
        public Task<T> DeleteAsync(T entity);
        public Task<T> DetailAsync(T entity);
        public Task<IEnumerable<T>> RetrieveAsync(T? entity);
        public Task<T> UpdateAsync(T entity);
    }
}
