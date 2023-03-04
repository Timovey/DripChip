using DripChip.DataContracts.Attributes;

namespace DripChip.DataContracts.DataContracts.Common
{
    public class CommonFilterContract
    {
        [Positive]
        public int From { get; set; } = 0;

        [GreaterThanZero]
        public int Size { get; set; } = 10;
    }
}
