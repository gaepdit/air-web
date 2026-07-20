using System;
using System.Collections.Generic;
using System.Text;

namespace AirWeb.Domain.Core.Entities;

public class UserPreferences
{
    public ThemePreference Theme { get; set; } = ThemePreference.Auto;
}
public enum ThemePreference
{
    [Display(Name = "Auto")]
    Auto = 0,

    [Display(Name = "Light")]
    Light = 1,

    [Display(Name = "Dark")]
    Dark = 2,
}
