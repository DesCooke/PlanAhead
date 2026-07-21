using PlanAhead.Core.Models.Base;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Models.Domain;

public class FundingRule : BaseEntity
{
    public Guid FundId { get; set; }

    public decimal Amount { get; set; }

    public Frequency Frequency { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Notes { get; set; } = "";

    public int? DayOfMonth { get; set; }

    public int? MonthOfYear { get; set; }

    public int? QuarterStartMonth { get; set; }
}