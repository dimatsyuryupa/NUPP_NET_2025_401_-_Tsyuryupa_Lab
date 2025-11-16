using Library.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LibraryContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(LibraryContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // Спеціальне завантаження навігації тільки для Book
            if (typeof(T) == typeof(Book))
            {
                return await _context.Books
                    .Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.Id == id) as T;
            }

            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Теж для Book
            if (typeof(T) == typeof(Book))
            {
                return await _context.Books
                    .Include(b => b.Author)
                    .ToListAsync() as IEnumerable<T>;
            }

            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity) =>
            await _dbSet.AddAsync(entity);

        public async Task UpdateAsync(T entity) =>
            _dbSet.Update(entity);

        public async Task DeleteAsync(T entity) =>
            _dbSet.Remove(entity);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
