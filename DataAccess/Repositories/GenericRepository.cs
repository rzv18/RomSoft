using System.Linq.Expressions;
using Business.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository()
        {
        }

        public IList<TEntity> GetAll()
        {
	        using var context = new ApplicationDbContext();
	        var dbSet = context.Set<TEntity>();
	        return dbSet.AsQueryable().ToList();
        }

        public IList<TEntity> FindBy(Expression<Func<TEntity, bool>>? predicate)
        {
            using var context = new ApplicationDbContext();
            var dbSet = context.Set<TEntity>();
            var query = dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }

        public void Add(TEntity entity)
        {
            using var context = new ApplicationDbContext();
            var dbSet = context.Set<TEntity>();
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Delete(TEntity entityToDelete)
        {
            using var context = new ApplicationDbContext();
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
            context.SaveChanges();
        }
    }
}
