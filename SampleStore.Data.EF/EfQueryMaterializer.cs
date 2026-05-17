using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace SampleStore.Data.EF;

public class EfQueryMaterializer : IQueryMaterializer
{
    public Task<int> Count<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken))
    {
        return query.CountAsync(cancellationToken);
    }

    public Task<List<TResult>> ToList<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken))
    {
        return query.ToListAsync(cancellationToken);
    }
}