using System;
using System.Collections.Generic;
using System.Text;
using PlanAhead.Core.Interfaces.Services;
using Microsoft.Maui.Networking;

namespace PlanAhead.Services
{

    public class ConnectivityService : IConnectivityService
    {
        public bool IsOnline =>
            Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
