using PlanAhead.core.Models.Enums;

namespace PlanAhead.core.Models.Configuration;

public class UserPreferences 
{
    public ProjectionMode DefaultProjectionMode { get; set; }

    public int ProjectionMonths { get; set; }

    public bool StartProjectionFromToday { get; set; }

    public bool ShowCompletedFunds { get; set; }

    public FundSortOrder DefaultFundSortOrder { get; set; }

    public bool UseDarkTheme { get; set; }

    public string CurrencySymbol { get; set; } = "£";
}