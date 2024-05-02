﻿namespace AirWeb.AppServices.WorkEntries.QueryDto;

public interface IBasicSearchDisplay
{
    SortBy Sort { get; }
    IDictionary<string, string?> AsRouteValues();
}
