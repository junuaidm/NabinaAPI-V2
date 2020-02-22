// <copyright file="ClientSourcedStringJsonConverter.cs" company="Nabin eCommerce API">
// Copyright (c) 2020 Nanina Trading Est. All rights reserved.
// </copyright>

namespace Nbn.eCommerce.ItemService.Security
{
    using Newtonsoft.Json;
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Custom JSON Converter that allows the C# 'string' type to be mapped and converted to a ClientSourcedString.
    /// </summary>
    internal sealed class ClientSourcedStringJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ClientSourcedString css = value as ClientSourcedString;
            if (css == null)
                throw new InvalidCastException();

            writer.WriteValue(css.ToTrustedString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            ClientSourcedString css = (string)reader.Value;
            return css;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ClientSourcedString);
        }
    }
}
