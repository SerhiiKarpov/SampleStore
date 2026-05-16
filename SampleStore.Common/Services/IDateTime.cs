
using System;

namespace SampleStore.Common.Services;
/// <summary>
/// An interface for date time.
/// </summary>
public interface IDateTime
{
    #region Properties

    /// <summary>
    /// Gets the UTC now.
    /// </summary>
    /// <value>
    /// The UTC now.
    /// </value>
    DateTime UtcNow
    {
        get;
    }

    #endregion Properties
}