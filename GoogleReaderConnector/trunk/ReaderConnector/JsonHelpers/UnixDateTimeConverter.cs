using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;

namespace CodeClimber.GoogleReaderConnector.JsonHelpers
{
    public class UnixDateTimeConverter: DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type t = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (reader.TokenType == JsonToken.Null)
            {
                if (!ReflectionUtils.IsNullableType(objectType))
                {
                    throw new Exception(String.Format("Cannot convert null value to {0}.",CultureInfo.InvariantCulture));
                }
                return null;
            }
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(String.Format("Unexpected token parsing date. Expected Integer, got {0}.",CultureInfo.InvariantCulture));
            }
            long ticks = (long)reader.Value;
            DateTime d = ticks.ConvertFromUnixTimestamp();
            if (t == typeof(DateTimeOffset))
            {
                return new DateTimeOffset(d);
            }
            return d;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                ticks = ((DateTime)value).ToUniversalTime().ConvertToUnixTimestamp();
            }
            else
            {
                if (!(value is DateTimeOffset))
                {
                    throw new Exception("Expected date object value.");
                }
                DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
                ticks = dateTimeOffset.ToUniversalTime().UtcDateTime.ConvertToUnixTimestamp();
            }
            writer.WriteValue(ticks);
        }

    }
}
