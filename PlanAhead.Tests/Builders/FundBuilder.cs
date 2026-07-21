using PlanAhead.Core.Models.Domain;

public class FundBuilder
{
    private readonly Fund _fund;

    private FundBuilder()
    {
        _fund = new Fund
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid(),
            Name = "New Fund",
            DisplayOrder = 1
        };
    }

    public static FundBuilder Create()
        => new();

    public FundBuilder WithName(string name)
    {
        _fund.Name = name;
        return this;
    }

    public FundBuilder ForAccount(Guid accountId)
    {
        _fund.AccountId = accountId;
        return this;
    }

    public FundBuilder DisplayOrder(int order)
    {
        _fund.DisplayOrder = order;
        return this;
    }

    public Fund Build()
        => _fund;
}