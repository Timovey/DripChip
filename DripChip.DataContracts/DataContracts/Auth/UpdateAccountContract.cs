using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class UpdateAccountContract
    {
        [FromRoute]
        [JsonPropertyName("accountId")]
        [GreaterThanZero]
        public int Id { get; set; }

        [FromBody]
        [NotSpace]
        public string FirstName { get; set; }

        [FromBody]
        [NotSpace]
        public string LastName { get; set; }

        [FromBody]
        [NotSpace]
        [EmailAddress]
        public string Email { get; set; }

        [FromBody]
        [NotSpace]
        public string Password { get; set; }
    }
}
