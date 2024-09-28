using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Reports.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T>  where T : class
    {
        IQueryable<T> GetAll();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}
