using DripChip.DataContracts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class ChangeTypeBody
    {
        [GreaterThanZero]
        public long OldTypeId { get; set; }

        [GreaterThanZero]
        public long NewTypeId { get; set; }
    }
}
