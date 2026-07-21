using PlanAhead.Core.Models.Base;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Models.Domain;

public class FundingRule : BaseEntity
{
    public Guid FundId { get; set; }

    public decimal Amount { get; set; }

    /// <summary>
    /// Canonical start of the occurrence this rule applies to.
    /// Examples:
    /// Annual:    2027-01-01
    /// Quarterly: 2027-04-01
    /// Monthly:   2027-03-01
    /// OneOff:    Actual event date
    /// </summary>
    public DateOnly StartDate { get; set; }

    public string Notes { get; set; } = "";
}