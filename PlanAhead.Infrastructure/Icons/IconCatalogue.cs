using PlanAhead.Core.Models.Icons;

namespace PlanAhead.Infrastructure.Icons;

public static class IconCatalogue
{
    public static IReadOnlyList<IconDefinition> All { get; } =
    [
        new()
        {
            Name = "Savings",
            DisplayName = "Savings",
            Category = "Money",
            ResourceName = "savings"
        },

        new()
        {
            Name = "Car",
            DisplayName = "Car",
            Category = "Transport",
            ResourceName = "car"
        },

        new()
        {
            Name = "Plane",
            DisplayName = "Plane",
            Category = "Travel",
            ResourceName = "plane"
        },

        new()
        {
            Name = "Tree",
            DisplayName = "Tree",
            Category = "Seasonal",
            ResourceName = "tree"
        },

        new()
        {
            Name = "Ring",
            DisplayName = "Ring",
            Category = "Family",
            ResourceName = "ring"
        }
    ];
}