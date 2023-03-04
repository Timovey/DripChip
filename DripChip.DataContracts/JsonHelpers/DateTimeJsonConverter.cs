using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DripChip.DataContracts.JsonHelpers
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private const string DATE_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{
        //    // implement in case you're serializing it back
        //}

        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        //    JsonSerializer serializer)
        //{
        //    var dataString = (string)reader.Value;
        //    DateTime date = parseDataString;

        //    return date;
        //}

        //public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    Debug.Assert(typeToConvert == typeof(DateTime));
        //    return DateTime.Parse(reader.GetString());
        //}

        //public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        //{
        //    writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        //}
        public override DateTime Read(
           ref Utf8JsonReader reader,
           Type typeToConvert,
           JsonSerializerOptions options) =>
               DateTime.ParseExact(reader.GetString()!,
                   DATE_FORMAT, CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
                  writer.WriteStringValue(dateTimeValue.ToUniversalTime().ToString(
                      DATE_FORMAT, CultureInfo.InvariantCulture));
        //writer.WriteStringValue(dateTimeValue.ToUniversalTime().ToString());
    }
}
