using System;
using System.Collections.Generic;

namespace CIR.Core.Entities.GlobalConfig;

public partial class GlobalConfigurationWeekends
{
    public long Id { get; set; }

    public long CountryId { get; set; }

    public long DayOfWeekId { get; set; }

    public virtual CountryCode Country { get; set; } = null!;
}
