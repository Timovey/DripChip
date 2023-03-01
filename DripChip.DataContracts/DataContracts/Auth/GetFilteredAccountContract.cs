using DripChip.DataContracts.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class GetFilteredAccountContract : CommonFilterContract
    {
        [FromQuery(Name = "firstName")]
        public string? FirstName { get; set; }

        [FromQuery(Name = "lastName")]
        public string? LastName { get; set; }

        [FromQuery(Name = "email")]
        public string? Email { get; set; }
    }
}
