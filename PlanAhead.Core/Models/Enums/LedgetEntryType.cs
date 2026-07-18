using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Models.Enums;

public enum LedgerEntryType
{
    Funding = 0,

    Expense = 1,

    Refund = 2,

    Adjustment = 3
}