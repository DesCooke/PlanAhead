using PlanAhead.Core.Models.Base;

namespace PlanAhead.Core.Models.Domain;

public class Account : BaseEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public decimal OpeningBalance { get; set; }

    public int DisplayOrder { get; set; }

    public string Notes { get; set; } = "";

    public bool Archived { get; set; }

    public string IconId { get; set; } = "Bank";

}