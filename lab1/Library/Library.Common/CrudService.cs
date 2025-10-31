namespace Library.Common
{
    public interface IEntity
    {
        Guid Id { get; }
    }

    public interface ICrudServiceAsync<T> where T : IEntity
    {
        Task<bool> CreateAsync(T element);
        Task<T> ReadAsync(Guid id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync();
    }
}
