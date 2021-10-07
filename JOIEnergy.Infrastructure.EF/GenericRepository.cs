using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOIEnergy.Domain;
using JOIEnergy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JOIEnergy.Infrastructure.EF
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly JOIEnergyDbContext _dbContext;

        public IUnitOfWork UnitOfWork => _dbContext;

        public GenericRepository(JOIEnergyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual TEntity Get(Guid id)
        {
            return _dbContext.Set<TEntity>().SingleOrDefault(x => x.Id == id);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return _dbContext.Set<TEntity>();
        }

        public void Create(IEnumerable<TEntity> aggregateRoots)
        {
            _dbContext.AddRange(aggregateRoots);
        }

        public void Update(IEnumerable<TEntity> aggregateRoots)
        {
            _dbContext.UpdateRange(aggregateRoots);
        }

        public void Update(TEntity aggregateRoot)
        {
            _dbContext.Set<TEntity>().Update(aggregateRoot);
        }

        public void Create(TEntity aggregateRoot)
        {
            _dbContext.Set<TEntity>().Add(aggregateRoot);
        }

        public void Delete(TEntity aggregateRoot)
        {
            _dbContext.Set<TEntity>().Remove(aggregateRoot);
        }

        public async Task CreateAsync(TEntity aggregateRoot)
        {
            await _dbContext.Set<TEntity>().AddAsync(aggregateRoot);
        }

        public async Task CreateAsync(IEnumerable<TEntity> aggregateRoots)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(aggregateRoots);
        }

        public Task<TEntity> GetAsync(Guid id)
        {
            return _dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
