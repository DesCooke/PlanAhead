using PlanAhead.Core.Models.Base;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Models.Domain;

public class Fund : BaseEntity
{
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public FundType FundType { get; set; }

    public FundStatus Status { get; set; }

    public Frequency Frequency { get; set; }

    public int? DueDay { get; set; }

    public int? DueMonth { get; set; }

    public int? QuarterStartMonth { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int DisplayOrder { get; set; }

    public string Notes { get; set; } = "";
}