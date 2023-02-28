using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.Attributes
{
    public class GreaterThanZeroOrNullAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value is int)
            {
                var x = (int)value;
                return x > 0;
            }
            else if (value is long)
            {
                var x = (long)value;
                return x > 0;
            }
            else if (value is double)
            {
                var x = (double)value;
                return x > 0;
            }
            else if (value is float)
            {
                var x = (float)value;
                return x > 0;
            }
            else if (value is decimal)
            {
                var x = (decimal)value;
                return x > 0;
            }

            return false;
        }
    }
}
