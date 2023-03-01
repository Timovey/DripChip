using DripChip.DataContracts.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.Auth
{
    public class CreateAccountContract
    {
        [NotSpace]
        public string FirstName { get; set; }

        [NotSpace]
        public string LastName { get; set; }

        [NotSpace]
        [EmailAddress]
        public string Email { get; set; }

        [NotSpace]
        public string Password { get; set; }
    }
}
