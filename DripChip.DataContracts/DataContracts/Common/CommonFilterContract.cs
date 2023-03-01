using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Common
{
    public class CommonFilterContract
    {
        [FromQuery]
        [Positive]
        public int From { get; set; } = 0;

        [FromQuery]
        [GreaterThanZero]
        public int Size { get; set; } = 10;
    }
}
