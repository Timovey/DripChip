using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class LoginContract
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
