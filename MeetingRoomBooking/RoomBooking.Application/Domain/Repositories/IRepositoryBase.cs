using RoomBooking.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IRepositoryBase<TEntity, TKey>
               where TEntity : class, IEntity<TKey>
               where TKey : IComparable
    {
        Task AddAsync(TEntity entity);
        Task EditAsync(TEntity entityToUpdate);
        Task<TEntity> GetByIdAsync(TKey id, bool isTracking = false);
        Task RemoveAsync(Expression<Func<TEntity, bool>> filter);
        Task RemoveAsync(TEntity entityToDelete);
        Task RemoveAsync(TKey id);

    }
}
