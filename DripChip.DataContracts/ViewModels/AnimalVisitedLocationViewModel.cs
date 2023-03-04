using DripChip.DataContracts.JsonHelpers;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.ViewModels
{
    public class AnimalVisitedLocationViewModel
    {
        public long Id { get; set; }

        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime DateTimeOfVisitLocationPoint { get; set; }

        public long LocationPointId { get; set; }
    }
}
