using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class GetFilteredAccountContract
    {
        [FromQuery]
        public string FirstName { get; set; }

        [FromQuery]
        public string LastName { get; set; }

        [FromQuery]
        public string Email { get; set; }

        [FromQuery]
        [Positive]
        public int From { get; set; }

        [FromQuery]
        [GreaterThanZero]
        public int Size { get; set; } = 10;

    }
}
