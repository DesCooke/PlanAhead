using PlanAhead.core.Models.Base;

namespace PlanAhead.core.Models.Domain;

public class FundTarget : BaseEntity
{
    public Guid FundId { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public decimal TargetAmount { get; set; }

    public string Notes { get; set; } = "";
}