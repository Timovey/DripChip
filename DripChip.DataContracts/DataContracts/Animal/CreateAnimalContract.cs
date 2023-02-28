using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class CreateAnimalContract
    {
        // Массив идентификаторов типов животного
        [Required]
        [FromBody]
        public long[] AnimalTypes { get; set; }

        // Масса животного, кг
        [FromBody]
        [GreaterThanZero]
        public float Weight { get; set; }

        // Длина животного, м
        [FromBody]
        [GreaterThanZero]
        public float Lenght { get; set; }

        // Высота животного, м
        [FromBody]
        [GreaterThanZero]
        public float Height { get; set; }

        // Гендерный признак животного, доступные значения “MALE”, “FEMALE”, “OTHER”
        [Required]
        [FromBody]
        public GenderType Gender { get; set; }

        // Жизненный статус животного, доступные значения
        // “ALIVE”(устанавливается автоматически при добавлении нового животного),
        // “DEAD”(можно установить при обновлении информации о животном)
        public LifeStatusType LifeStatus { get; } = LifeStatusType.ALIVE;

        // Дата и время чипирования в формате ISO-8601
        // (устанавливается автоматически на момент добавления животного)
        public DateTime ChippingDateTime { get; } = DateTime.Now;

        // Идентификатор аккаунта пользователя
        [FromBody]
        [GreaterThanZero]
        public int ChipperId { get; set; }

        // Идентификатор точки локации животных
        [FromBody]
        [GreaterThanZero]
        public long ChippingLocationId { get; set; }
    }
}
