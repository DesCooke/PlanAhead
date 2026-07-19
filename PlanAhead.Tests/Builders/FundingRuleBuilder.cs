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
            Frequency = Frequency.Monthly,
            StartDate = new DateOnly(2027, 1, 1)
        };
    }

    public static FundingRuleBuilder Create()
    {
        return new FundingRuleBuilder();
    }

    public static FundingRuleBuilder Monthly()
    {
        return Create().WithFrequency(Frequency.Monthly);
    }

    public static FundingRuleBuilder Quarterly()
    {
        return Create().WithFrequency(Frequency.Quarterly);
    }

    public static FundingRuleBuilder BiAnnual()
    {
        return Create().WithFrequency(Frequency.BiAnnual);
    }

    public static FundingRuleBuilder Annual()
    {
        return Create().WithFrequency(Frequency.Annual);
    }

    public static FundingRuleBuilder OneOff()
    {
        return Create().WithFrequency(Frequency.OneOff);
    }

    public FundingRuleBuilder WithFrequency(
        Frequency frequency)
    {
        _rule.Frequency = frequency;
        return this;
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

    public FundingRuleBuilder EndingOn(
        int year,
        int month,
        int day)
    {
        _rule.EndDate = new DateOnly(year, month, day);
        return this;
    }

    public FundingRuleBuilder EndingOn(
        DateOnly date)
    {
        _rule.EndDate = date;
        return this;
    }

    public FundingRuleBuilder WithoutEndDate()
    {
        _rule.EndDate = null;
        return this;
    }

    public FundingRuleBuilder WithAmount(
        decimal amount)
    {
        _rule.Amount = amount;
        return this;
    }

    public FundingRuleBuilder ForAccount(Guid accountId)
    {
        _rule.AccountId = accountId;
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

    public FundingRuleBuilder OnDay(int day)
    {
        _rule.DayOfMonth = day;
        return this;
    }

    public FundingRuleBuilder InMonth(int month)
    {
        _rule.MonthOfYear = month;
        return this;
    }

    public FundingRuleBuilder StartingQuarterIn(int month)
    {
        _rule.QuarterStartMonth = month;
        return this;
    }

    public FundingRule Build()
    {
        return _rule;
    }
}