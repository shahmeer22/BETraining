using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Training.Data;
using System.Linq.Dynamic.Core;
using Training.Models;
using Training.Constants;

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

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
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

        public async Task<(IQueryable<T>, int)> GetPaginatedResult(IQueryable<T> query, PaginationQueryParams param)
        {
            // Search
            if (!string.IsNullOrEmpty(param.Search))
            {
                string[] conditions = param.Search.Split("&&");

                foreach (string condition in conditions)
                {
                    string[] parts = condition.Split('=', 2);

                    if (parts.Length == 2)
                    {
                        string name = parts[0].Trim();
                        string value = parts[1].Trim();

                        if (int.TryParse(value, out int val))
                        {
                            query = query.Where($"{name} == @0", val);
                        }
                        else
                        {
                            query = query.Where($"Convert.ToString({name}).Contains(@0)", value);
                        }
                    }
                }
            }

            // Sort
            string columnSort = param.Descending ? $"{param.SortBy} descending" : param.SortBy;
            query = query.OrderBy(columnSort);

            // Pagination
            int totalItems = await query.CountAsync();
            GetStartAndCount(totalItems, param.Page, param.PageSize, out int start, out int count);
            query = query.Skip(start).Take(count);

            return (query, totalItems);
        }

        public void GetStartAndCount(int totalItems, int page, int pageSize, out int start, out int count)
        {
            if (totalItems == 0)
            {
                start = 0;
                count = 0;
                return;
            }

            if (page <= 0) throw new Exception(ExceptionMessages.PAGE_GREATER_THAN_ZERO);
            if (pageSize <= 0 || pageSize > 500) throw new Exception(ExceptionMessages.INCORRECT_PAGE_SIZE);

            start = (page - 1) * pageSize;
            if (start >= totalItems) throw new Exception(ExceptionMessages.PAGE_DOES_NOT_EXIST);

            count = Math.Min(pageSize, totalItems - start);
        }
    }
}
