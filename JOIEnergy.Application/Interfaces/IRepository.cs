using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOIEnergy.Domain;

namespace JOIEnergy.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IUnitOfWork UnitOfWork { get; }
        TEntity Get(Guid id);
        IQueryable<TEntity> Query();
        void Create(TEntity aggregateRoot);
        void Create(IEnumerable<TEntity> aggregateRoots);
        void Update(TEntity aggregateRoot);
        void Update(IEnumerable<TEntity> aggregateRoots);
        void Delete(TEntity aggregateRoot);
        Task<TEntity> GetAsync(Guid id);
        Task CreateAsync(TEntity aggregateRoot);
        Task CreateAsync(IEnumerable<TEntity> aggregateRoots);
    }
}
