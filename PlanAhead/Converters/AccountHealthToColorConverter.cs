using PlanAhead.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PlanAhead.Converters
{
    public class AccountHealthToColorConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value switch
            {
                Status.Green => Colors.Green,
                Status.Amber => Colors.Orange,
                Status.Red => Colors.Red,
                _ => Colors.Transparent
            };
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
