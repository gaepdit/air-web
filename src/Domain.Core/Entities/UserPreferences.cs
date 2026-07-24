using System;
using System.Collections.Generic;
using System.Text;

namespace AirWeb.Domain.Core.Entities;

public class UserPreferences
{
    public ThemePreference Theme { get; set; } = ThemePreference.Dark;
}
public enum ThemePreference
{
    Auto = 0,
    Light = 1,
    Dark = 2,
}
