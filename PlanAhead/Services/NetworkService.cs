using Microsoft.Maui.Networking;
using PlanAhead.Core.Interfaces.Services;

public class NetworkService : INetworkService
{
    public bool IsConnected =>
        Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
}