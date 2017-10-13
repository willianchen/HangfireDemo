using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobExtensions
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：TimeZoneInfoConverter.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：TimeZoneInfoConverter
    /// 创建标识：cml 2017/10/13 15:47:58
    /// </summary>
    class TimeZoneInfoConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified <see cref="TimeZoneInfo"/>
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified <see cref="TimeZoneInfo"/>; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeZoneInfo);
        }
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from</param>
        /// <param name="objectType">Type of the object</param>
        /// <param name="existingValue">The existing value of object being read</param>
        /// <param name="serializer">The calling serializer</param>
        /// <returns>The object value</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            if (reader.Value == null) return null;

            return TimeZoneInfo.FindSystemTimeZoneById(reader.Value.ToString());
        }
        /// <summary>
        /// Writes the JSON representation of the <see cref="TimeZoneInfo"/>.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to</param>
        /// <param name="value">The value</param>
        /// <param name="serializer">The calling serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var o = value as TimeZoneInfo;

            writer.WriteValue(o.StandardName);
        }
    }
}