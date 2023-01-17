using System.Linq.Expressions;

namespace Business.Repositories.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        IList<T> GetAll();

        IList<T> FindBy(Expression<Func<T, bool>>? predicate);

        void Add(T entity);

        void Delete(T entity);
    }
}
