using API_TEST.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_TEST.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll();
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }


    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly MyDBContext dbContext;
        public RepositoryBase(MyDBContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public T Create(T entity) => dbContext.Set<T>().Add(entity).Entity;

        public void Delete(T entity) => dbContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll() => dbContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => dbContext.Set<T>().Where(expression).AsNoTracking();

        public T Update(T entity) => dbContext.Set<T>().Update(entity).Entity;

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbContext.Set<T>().AsQueryable();
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }
    }
}
