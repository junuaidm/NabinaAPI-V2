// <copyright file="ClientSourcedStringTypeConverter.cs" company="Nabin eCommerce API">
// Copyright (c) 2020 Nanina Trading Est. All rights reserved.
// </copyright>

namespace Nbn.eCommerce.ItemService.Security
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// This is a custom type converter that allows MVC parameter binding to map 'string' to ClientSourcedString.
    /// </summary>
    public class ClientSourcedStringTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
           System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return new ClientSourcedString((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
