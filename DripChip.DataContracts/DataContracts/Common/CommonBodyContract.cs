using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Common
{
    public class CommonBodyContract<T>
    {
        [FromBody]
        public T Body { get; set; }
    }
}
