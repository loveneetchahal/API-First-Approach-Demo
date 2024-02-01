using APIFirstDemo.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APIFirstDemo.Services
{
    public abstract class GenericRepositoryBase<T> : IGenericRepositoryBase<T> where T : class
    {
        protected DemoDbContext _dbContext { get; set; }

        public GenericRepositoryBase(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> FindAll()
        {
            return _dbContext.Set<T>()
                .AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>()
                .Where(expression)
                .AsNoTracking();
        }

        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    } 
}
