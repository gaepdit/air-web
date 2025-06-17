﻿namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IResponseRequested
{
    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
    public string? ResponseComment { get; set; }
}
