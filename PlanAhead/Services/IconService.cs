using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Services
{
    public static class IconService
    {
        public static ImageSource Get(string iconName)
        {
            return ImageSource.FromFile(iconName);
        }
    }
}
