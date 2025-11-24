using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Training.Data;

namespace Training.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        public T Update(T entity) {
            _dbSet.Update(entity);
            return entity;
        }
        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }
    }
}
