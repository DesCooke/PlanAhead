using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Tests.Builders;

public class FundingRuleBuilder
{
    private readonly FundingRule _rule;

    public FundingRuleBuilder()
    {
        _rule = new FundingRule
        {
            Id = Guid.NewGuid(),
            StartDate = new DateOnly(2027, 1, 1)
        };
    }

    public static FundingRuleBuilder Create()
    {
        return new FundingRuleBuilder();
    }


    public FundingRuleBuilder StartingOn(
        int year,
        int month,
        int day)
    {
        _rule.StartDate = new DateOnly(year, month, day);
        return this;
    }

    public FundingRuleBuilder StartingOn(
        DateOnly date)
    {
        _rule.StartDate = date;
        return this;
    }

    public FundingRuleBuilder WithAmount(
        decimal amount)
    {
        _rule.Amount = amount;
        return this;
    }

    public FundingRuleBuilder ForFund(Guid fundId)
    {
        _rule.FundId = fundId;
        return this;
    }

    public FundingRuleBuilder WithNotes(string notes)
    {
        _rule.Notes = notes;
        return this;
    }

    public FundingRule Build()
    {
        return _rule;
    }
}