using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities;

namespace SampleStore.Data.Extensions;

public static class UnitOfWorkExtensions
{
        public static async Task Update<TEntity>(
        this IUnitOfWork unitOfWork,
        TEntity entity,
        IQueryMaterializer queryMaterializer,
        CancellationToken cancellationToken = default(CancellationToken))
        where TEntity : Entity
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(unitOfWork);

        var repository = unitOfWork.GetRepository<TEntity>();
        var query = repository.Query.Where(e => e.Id == entity.Id);
        var found = await queryMaterializer.FirstOrDefault(query, cancellationToken);
        if (found == null)
        {
            return;
        }

        entity.CopyTo(found);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}