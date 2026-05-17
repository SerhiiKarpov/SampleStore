using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SampleStore.Common.Extensions;

namespace SampleStore.Data.EF;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public EfUnitOfWork(DbContext context)
    {
        _context = context.ThrowIfArgumentIsNull(nameof(context));
    }

    public IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class
    {
        return new EfRepository<TEntity>(this, _context.Set<TEntity>());
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException x)
        {
            var conflictedEntities = x.Entries.Select(entry => entry.Entity);
            throw new ConcurrencyException(conflictedEntities);
        }
    }
}