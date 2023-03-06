using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.JsonHelpers
{
    public class NullDateTimeJsonConverter : JsonConverter<DateTime?>
    {
        private const string DATE_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";

        public override DateTime? Read(
           ref Utf8JsonReader reader,
           Type typeToConvert,
           JsonSerializerOptions options)
        {
            var str = reader.GetString()!;
            return str != null ?
                DateTime.ParseExact(str, DATE_FORMAT, CultureInfo.InvariantCulture) :
                null;
        }
              

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? dateTimeValue,
            JsonSerializerOptions options)
        {
            if (dateTimeValue.HasValue)
            {
                writer.WriteStringValue(dateTimeValue.Value.ToUniversalTime().ToString(
                      DATE_FORMAT, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
                  
    }
}
