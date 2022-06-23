namespace ModelApp.Service.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        public Task<T> AddAsync(T entity);
        public Task<T> RefreshAsync(int id, T entity);
        public Task<T> RemoveAsync(int id);
        public Task<IEnumerable<T>> GetAllAsync(T? entity = null);
        public Task<T> GetAsync(T entity);
    }
}
