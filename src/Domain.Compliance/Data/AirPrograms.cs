using System;
using System.Collections.Generic;
using System.Text;

namespace AirWeb.Domain.Compliance.Data;

public static class CommonoData
{
    public static ICollection<AirPrograms> AsAirPrograms(this List<string> airprogramIds) =>
        AllAirPrograms.Where(airprogram => airprogramIds.Contains(airprogram.Code)).ToList();
    private static List<AirPrograms> AllAirPrograms { get; } =
    [
        new("CAASIP", "SIP"),
        new("CAAFIP","Federal SIP"),
        new("CAANFRP","Non-Federal SIP"),
        new("CAACFC","CFC Tracking"),
        new("CAAPSD","PSD"),
        new("CAANSR","NSR"),
        new("CAANESH","NESHAP"),
        new("CAANSPS","ASPS"),
        new("CAAAR","Acid Precipitation"),
        new("CAAFENF","Federally-Enforceable Requirement"),
        new("CAAFESOP","FESOP"),
        new("CAAGHG","Greenhouse Gas Reporting Rule"),
        new("CAANAM","Native American"),
        new("CAAMACT","MACT"),
        new("CAAPARGDC","Prevention of Accidental Release"),
        new("CAATIP","Tribal Implementation Plan (TIP)"),
        new("CAATVP","Title V"),
        new("CAAGACTM","40 CFR Part 63 Area Sources"),
        new("CAANSPSM","NSPS (Non-Major)"),
        new("CAARMP","Risk Management Program"),
    ];
}
