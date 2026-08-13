using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Models.Authentication
{
    public class UserInfo
    {
        public Guid Id { get; init; }

        public string Email { get; init; } = "";
    }
}
