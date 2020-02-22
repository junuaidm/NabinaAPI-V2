// <copyright file="ClientSourcedString.cs" company="Nabin eCommerce API">
// Copyright (c) 2020 Nanina Trading Est. All rights reserved.
// </copyright>

namespace Nbn.eCommerce.ItemService.Security
{
    using System.ComponentModel;
    using System.Web;
    using Microsoft.Security.Application;
    using Newtonsoft.Json;

    /// <summary>
    /// This class ensures that the string content passed to its consturctor when it's created
    /// is Html Encoded when it is accessed.  This is to prevent CrLf injection attacks in our code.
    /// </summary>
    [JsonConverter(typeof(ClientSourcedStringJsonConverter))]
    [TypeConverter(typeof(ClientSourcedStringTypeConverter))]
    public sealed class ClientSourcedString
    {
        private string _trusted;
        private string _untrusted;

        /// <summary>
        /// Implicit operator overload for equating a string to a ClientSourcedString
        /// </summary>
        /// <param name="value">Untrusted string value</param>
        public static implicit operator ClientSourcedString(string value)
        {
            return new ClientSourcedString(HttpUtility.HtmlDecode(value));
        }

        /// <summary>
        /// Implicit operator overload for equating a ClientSourcedString to a string
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator string(ClientSourcedString source)
        {
            return (source == null) ? null : source.ToTrustedString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSourcedString"/> class.
        /// Default Constructor
        /// </summary>
        public ClientSourcedString()
        {
            _trusted = null;
            _untrusted = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSourcedString"/> class.
        /// Constructor
        /// </summary>
        /// <param name="value">Untrusted source string</param>
        public ClientSourcedString(string value)
        {
            if (value == null)
            {
                _trusted = null;
            }
            else
            {
                _trusted = string.Empty;
            }
            _untrusted = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSourcedString"/> class.
        /// Copy Constructor
        /// </summary>
        /// <param name="value">Another instance of a ClientSourcedString</param>
        public ClientSourcedString(ClientSourcedString value)
        {
            // Protect against self reference.
            if (this != value)
            {
                this._trusted = value._trusted;
                this._untrusted = value._untrusted;
            }
        }

        /// <summary>
        /// Explicit Method for converting untrusted to trusted content
        /// </summary>
        /// <returns></returns>
        public string ToTrustedString()
        {
            if (!string.IsNullOrEmpty(_untrusted) && string.IsNullOrEmpty(_trusted))
            {
                // We want to HTMLEncode most characters, but we also need
                // single quotes to come through unmodified.
                _trusted = Encoder.HtmlEncode(_untrusted).Replace("&#39;", "\'");
            }

            return _trusted;
        }

        /// <summary>
        /// ToString overload
        /// </summary>
        /// <returns>HTML encoded string</returns>
        public override string ToString()
        {
            return this.ToTrustedString();
        }
    }
}
