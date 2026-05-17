using System;

namespace SampleStore.Common.Services;

public interface IDateTime
{
    DateTime UtcNow { get; }
}