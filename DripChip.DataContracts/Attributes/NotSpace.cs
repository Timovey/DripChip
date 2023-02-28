using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.Attributes
{
    public class NotSpace : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value is string)
            {
                string x = value.ToString();
                return x.Trim().Length > 0;
            }
            return false;
        }
    }
}
