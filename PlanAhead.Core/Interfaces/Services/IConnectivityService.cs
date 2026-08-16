using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Services
{
    public interface IConnectivityService
    {
        bool IsOnline { get; }
    }
}
