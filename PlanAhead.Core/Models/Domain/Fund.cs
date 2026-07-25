using PlanAhead.Core.Models.Base;
using PlanAhead.Core.Models.Enums;

public class Fund : BaseEntity
{
    public Guid AccountId { get; set; }

    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public Frequency Frequency { get; set; }

    public FundStatus Status { get; set; }

    public int DisplayOrder { get; set; }

    public string Notes { get; set; } = "";

    public string IconId { get; set; } = "PiggyBank";

}