using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Library.Common;
using Library.Infrastructure.Repositories;

namespace Library.Infrastructure.Services
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class, IEntity
    {
        private readonly IRepository<T> _repository;

        public CrudServiceAsync(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<T> ReadAsync(Guid id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<T>> ReadAllAsync() => await _repository.GetAllAsync();

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await _repository.GetAllAsync();
            return all.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.UpdateAsync(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.DeleteAsync(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
