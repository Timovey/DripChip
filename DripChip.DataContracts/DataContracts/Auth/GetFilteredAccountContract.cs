using DripChip.DataContracts.DataContracts.Common;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class GetFilteredAccountContract : CommonFilterContract
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
    }
}
