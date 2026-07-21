using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Tests.Builders;

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
            Description = "",
            Status = FundStatus.Active,
            DisplayOrder = 1,
            Notes = ""
        };
    }

    public static FundBuilder Create()
    {
        return new FundBuilder();
    }

    public FundBuilder ForAccount(Guid accountId)
    {
        _fund.AccountId = accountId;
        return this;
    }

    public FundBuilder WithName(string name)
    {
        _fund.Name = name;
        return this;
    }

    public FundBuilder WithDescription(string description)
    {
        _fund.Description = description;
        return this;
    }


    public FundBuilder WithStatus(FundStatus status)
    {
        _fund.Status = status;
        return this;
    }

    public FundBuilder WithDisplayOrder(int displayOrder)
    {
        _fund.DisplayOrder = displayOrder;
        return this;
    }

    public FundBuilder WithNotes(string notes)
    {
        _fund.Notes = notes;
        return this;
    }

    public FundBuilder WithFrequency(Frequency frequency)
    {
        _fund.Frequency = frequency;
        return this;
    }

    public Fund Build()
    {
        return _fund;
    }
}