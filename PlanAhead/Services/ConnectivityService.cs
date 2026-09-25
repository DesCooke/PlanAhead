using Microsoft.Maui.Networking;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Services
{
    public class ConnectivityService : IConnectivityService
    {
        public bool IsOnline =>
            Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
