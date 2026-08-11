using System;
using System.Collections.Generic;
using System.Text;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.ViewModels.Accounts
{
    public class AccountListItem
    {
        public Account Account { get; init; } = null!;

        public Status Status { get; init; }

        public string IconId => Account.IconId;
        public string Name => Account.Name;
        public string Description => Account.Description;
        public string OpeningBalanceDisplay => Account.OpeningBalance.ToString("C");
    }
}
