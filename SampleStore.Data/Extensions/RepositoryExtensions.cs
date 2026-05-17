using SampleStore.Common.Extensions;

namespace SampleStore.Data.Extensions;

public static class RepositoryExtensions
{
    public static void Add<TEntity>(this IRepository<TEntity> repository, TEntity entity)
        where TEntity : class
    {
        repository.ThrowIfArgumentIsNull(nameof(repository));
        entity.ThrowIfArgumentIsNull(nameof(entity));
        repository.Add(entity.ToEnumerable());
    }

    public static void Remove<TEntity>(this IRepository<TEntity> repository, TEntity entity)
        where TEntity : class
    {
        if (entity == null)
        {
            return;
        }

        repository.ThrowIfArgumentIsNull(nameof(repository));
        repository.Remove(entity.ToEnumerable());
    }
}