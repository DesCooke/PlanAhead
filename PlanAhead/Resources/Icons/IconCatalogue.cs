using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Resources.Icons
{
    public static class IconCatalogue
    {
        private static readonly List<IconDefinition> _icons =
        [
            new()
        {
            Id = "PiggyBank",
            DisplayName = "Piggy Bank",
            ResourceName = "piggy_bank",
            Categories = ["Finance"],
            Keywords = ["money", "saving", "bank", "cash", "coin"]
        },

        new()
        {
            Id = "Plane",
            DisplayName = "Plane",
            ResourceName = "plane",
            Categories = ["Travel"],
            Keywords = ["holiday", "flight", "vacation"]
        },

        new()
        {
            Id = "Bank",
            DisplayName = "Bank",
            ResourceName = "bank",
            Categories = ["finance"],
            Keywords = ["bank"]
        }


        ];

        public static IReadOnlyList<IconDefinition> All => _icons;

        public static IconDefinition? Get(string id)
        {
            return _icons.FirstOrDefault(i =>
                i.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public static string GetResourceName(string id)
        {
            return Get(id)?.ResourceName ?? "piggy_bank";
        }
    }
}
