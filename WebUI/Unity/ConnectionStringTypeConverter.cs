using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.Configuration;

namespace WebUI
{
    public class ConnectionStringTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return ConfigurationManager.ConnectionStrings[value.ToString()];
        }
    }
}