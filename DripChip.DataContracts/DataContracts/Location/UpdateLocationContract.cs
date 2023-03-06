using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Location
{
    public class UpdateLocationContract : CommonBodyContract<LocationBody>
    {
        [FromRoute]
        [GreaterThanZero]
        public long PointId { get; set; }
    }
}
