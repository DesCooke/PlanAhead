using PlanAhead.core.Models.Base;
using PlanAhead.core.Models.Enums;

namespace PlanAhead.core.Models.Domain;

public class FundingRule : BaseEntity
{
    public Guid AccountId { get; set; }

    public Guid FundId { get; set; }

    public decimal Amount { get; set; }

    public Frequency Frequency { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Notes { get; set; } = "";
}