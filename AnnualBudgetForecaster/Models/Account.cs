using SQLite;

namespace PlanAhead.Models;

[Table("Accounts")]
public class Account : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; } = "";

    public decimal Balance { get; set; }

    public bool IncludeInTotal { get; set; } = true;

    public int DisplayOrder { get; set; }
}