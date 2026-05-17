using System;
using System.Collections;
using System.Linq;

using SampleStore.Common.Extensions;

namespace SampleStore.Data;

public class ConcurrencyException : Exception
{
    private const string DefaultMessage = "One or more entities were updated or deleted by other process.";

    public ConcurrencyException(IEnumerable conflictedEntities)
        : this(DefaultMessage)
    {
        ConflictedEntities = conflictedEntities ?? throw new ArgumentNullException(nameof(conflictedEntities));
    }

    public ConcurrencyException(string message)
        : base(message)
    {
        ConflictedEntities = Enumerable.Empty<object>();
    }

    public ConcurrencyException(string message, Exception innerException)
        : base(message, innerException)
    {
        ConflictedEntities = Enumerable.Empty<object>();
    }

    public ConcurrencyException()
        : this(Enumerable.Empty<object>())
    {
    }

    public IEnumerable ConflictedEntities { get; }
}