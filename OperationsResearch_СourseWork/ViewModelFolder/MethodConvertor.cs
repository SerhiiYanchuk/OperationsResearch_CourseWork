using System;
using System.Globalization;
using System.Windows.Data;
using static OperationsResearch_СourseWork.TRPZSolution;

namespace OperationsResearch_СourseWork
{
    class MethodConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int integer = (int)((Method)value);
            if (integer == int.Parse(parameter.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
