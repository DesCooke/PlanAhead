using Microsoft.Maui.Networking;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.MethodLogging;

[MethodLogging]
public class NetworkService : INetworkService
{
    public bool IsConnected =>
        Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
}