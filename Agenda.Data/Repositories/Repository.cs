using Agenda.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agenda.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public readonly DbContext _context;

        protected readonly DbSet<TEntity> DbSet;

        public Repository(DbContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity obj)
        {
            DbSet.Add(obj);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual TEntity GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public void Remove(Guid id)
        {
            var entity = DbSet.Find(id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(TEntity obj)
        {
            DbSet.Update(obj);
            _context.SaveChanges();
        }
    }
}
