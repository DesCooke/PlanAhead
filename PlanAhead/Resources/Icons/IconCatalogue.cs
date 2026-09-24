using PlanAhead.Core.Logging;
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
            String? ret = id;
            using var log = MethodLoggingService.Begin();
            try
            {
                if (id == null || id.Length == 0)
                {
                    ret = "piggy_bank";
                }
                else
                {

                    var iconDef = Get(id);

                    if (iconDef == null)
                    {
                        ret = id;
                    }
                    else
                    {
                        ret = iconDef.ResourceName;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                throw;
            }
            MethodLoggingService.Write($"  Returning {ret}");
            return ret;
        }
    }
}
