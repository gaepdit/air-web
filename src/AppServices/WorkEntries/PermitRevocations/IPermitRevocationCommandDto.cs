﻿namespace AirWeb.AppServices.WorkEntries.PermitRevocations;

public interface IPermitRevocationCommandDto
{
    public DateOnly ReceivedDate { get; }
    public DateOnly PermitRevocationDate { get; }
    public DateOnly? PhysicalShutdownDate { get; }
    public bool FollowupTaken { get; }
}
