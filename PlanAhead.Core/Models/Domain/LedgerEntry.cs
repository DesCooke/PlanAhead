using PlanAhead.Core.Models.Base;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Models.Domain;

public class LedgerEntry : BaseEntity
{
    public Guid AccountId { get; set; }

    public Guid? FundId { get; set; }

    public LedgerEntryType EntryType { get; set; }

    public DateOnly EntryDate { get; set; }

    public DateOnly? BudgetPeriodDate { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = "";

    public string Notes { get; set; } = "";

}