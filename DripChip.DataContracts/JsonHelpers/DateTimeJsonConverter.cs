using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DripChip.DataContracts.JsonHelpers
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private const string DATE_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
     
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
