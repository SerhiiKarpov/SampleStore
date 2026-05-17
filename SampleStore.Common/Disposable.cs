using System;

namespace SampleStore.Common;

public abstract class Disposable : IDisposable
{
    ~Disposable()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Override to free resources.
    }
}