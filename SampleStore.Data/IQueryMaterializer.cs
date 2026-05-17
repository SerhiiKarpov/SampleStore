using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Data;

public interface IQueryMaterializer
{
    Task<int> Count<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken));

    Task<List<TResult>> ToList<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken));
}