namespace Library.Common
{
    public interface IEntity
    {
        int Id { get; }
    }

    public interface ICrudServiceAsync<T> where T : IEntity
    {
        Task<bool> CreateAsync(T element);
        Task<T> ReadAsync(int id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync();
    }
}
