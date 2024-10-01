using SecTech.Reports.DAL;
using SecTech.Reports.Domain.Interfaces.Repository;

namespace SecTech.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<T> CreateAsync(T entity)
        {
            if (entity == null) { throw new ArgumentNullException(); }
            _context.Add(entity);
            _context.SaveChanges();
            return Task.FromResult(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public Task RemoveAsync(T entity)
        {
            if (entity == null) { throw new ArgumentNullException(); }
            _context.Remove(entity);
            _context.SaveChanges();
            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            if (entity == null) { throw new ArgumentNullException(); }
            _context.Update(entity);
            _context.SaveChanges();
            return Task.FromResult(entity);
        }
    }
}
