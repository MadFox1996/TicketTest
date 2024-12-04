namespace TaskApi.DAL.Interfaces
{
    public interface IRepository<T, ID> where T : class
    {
        Task<T?> GetAsync(int id);

        Task CreateAsync(ICollection<T> items);
        Task CreateAsync(T item);
        Task<int> UpdateAsync(T item);
        Task<bool> DeleteAsync(ICollection<ID> id);
    }
}
