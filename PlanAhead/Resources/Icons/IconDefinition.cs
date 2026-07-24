using PlanAhead.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Resources.Icons
{
    public sealed class IconDefinition
    {
        public string Id { get; init; } = "";

        public string DisplayName { get; init; } = "";

        public string ResourceName { get; init; } = "";

        public IReadOnlyList<string> Categories { get; init; } = [];

        public IReadOnlyList<string> Keywords { get; init; } = [];
    }
}
