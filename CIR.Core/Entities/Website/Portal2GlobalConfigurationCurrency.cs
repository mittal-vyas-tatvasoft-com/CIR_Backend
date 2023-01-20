using CIR.Core.Entities.GlobalConfiguration;
using System;
using System.Collections.Generic;

namespace CIR.Core.Entities.Websites;

public partial class Portal2GlobalConfigurationCurrency
{
    public long Id { get; set; }

    public long PortalId { get; set; }

    public long GlobalConfigurationCurrencyId { get; set; }

    public bool EnabledOverride { get; set; }

}
