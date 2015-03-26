using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using FET.Data;

namespace FET.Convertor
{
    class ActivityTagToText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is ActivityTag)
            {
                ActivityTag act = value as ActivityTag;
                return act.Name;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string activityTagName = (string)value;

            foreach (var act in Timetable.GetInstance().ActivitiyTagsList)
            {
                if (act.Name.Equals(activityTagName))
                {
                    return act;
                }
            }

            return null;
        }
    }
}
