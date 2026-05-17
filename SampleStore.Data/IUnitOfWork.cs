using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Data;

public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class;

    Task SaveChanges(CancellationToken cancellationToken = default(CancellationToken));
}