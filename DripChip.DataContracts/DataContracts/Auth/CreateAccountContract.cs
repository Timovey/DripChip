using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class CreateAccountContract
    {
        [NotSpace]
        [FromBody]
        public string FirstName { get; set; }

        [NotSpace]
        [FromBody]
        public string LastName { get; set; }

        [NotSpace]
        [FromBody]
        public string Email { get; set; }

        [NotSpace]
        [FromBody]
        public string Password { get; set; }
    }
}
