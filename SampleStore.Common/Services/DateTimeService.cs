using System;

namespace SampleStore.Common.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow
    {
        get
        {
            return DateTime.UtcNow;
        }
    }
}