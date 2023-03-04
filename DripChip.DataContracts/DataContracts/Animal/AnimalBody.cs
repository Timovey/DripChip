using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class AnimalBody
    {
        // Масса животного, кг
        [GreaterThanZero]
        public float Weight { get; set; }

        // Длина животного, м
        [GreaterThanZero]
        public float Length { get; set; }

        // Высота животного, м
        [GreaterThanZero]
        public float Height { get; set; }

        // Гендерный признак животного, доступные значения “MALE”, “FEMALE”, “OTHER”
        [Required]
        public GenderType Gender { get; set; }

        // Жизненный статус животного, доступные значения
        // “ALIVE”(устанавливается автоматически при добавлении нового животного),
        // “DEAD”(можно установить при обновлении информации о животном)
        [Required]
        public LifeStatusType LifeStatus { get; set; }

        // Идентификатор аккаунта пользователя
        [GreaterThanZero]
        public int ChipperId { get; set; }

        // Идентификатор точки локации животных
        [GreaterThanZero]
        public long ChippingLocationId { get; set; }
    }
}
