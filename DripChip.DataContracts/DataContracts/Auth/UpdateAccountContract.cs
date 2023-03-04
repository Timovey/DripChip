using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class UpdateAccountContract :  CommonBodyContract<AccountBody>
    {
        [FromRoute(Name = "accountId")]
        [GreaterThanZero]
        public int AccountId { get; set; }
    }
}
