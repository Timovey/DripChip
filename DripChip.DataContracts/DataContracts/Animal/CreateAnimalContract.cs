using DripChip.DataContracts.Attributes;
using DripChip.DataContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace DripChip.DataContracts.DataContracts.Animal
{
    public class CreateAnimalContract
    {
        // Массив идентификаторов типов животного
        [Required]
        public List<long> AnimalTypes { get; set; }

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
        public GenderType Gender { get; set; }

        // Жизненный статус животного, доступные значения
        // “ALIVE”(устанавливается автоматически при добавлении нового животного),
        // “DEAD”(можно установить при обновлении информации о животном)
        public LifeStatusType LifeStatus { get; } = LifeStatusType.ALIVE;

        // Дата и время чипирования в формате ISO-8601
        // (устанавливается автоматически на момент добавления животного)
        public DateTime ChippingDateTime { get; } = DateTime.UtcNow;

        // Идентификатор аккаунта пользователя
        [GreaterThanZero]
        public int ChipperId { get; set; }

        // Идентификатор точки локации животных
        [GreaterThanZero]
        public long ChippingLocationId { get; set; }

        public List<long> VisitedLocations { get; } = new List<long>();
    }
}
