﻿using DripChip.DataContracts.Enums;

namespace DripChip.DataContracts.ViewModels
{
    public class AnimalViewModel
    {
        public long Id { get; set; }

        // Массив идентификаторов типов животного
        public long[] AnimalTypes { get; set; }

        // Масса животного, кг
        public float Weight { get; set; }

        // Длина животного, м
        public float Lenght { get; set; }

        // Высота животного, м
        public float Height { get; set; }

        // Гендерный признак животного, доступные значения “MALE”, “FEMALE”, “OTHER”
        public GenderType Gender { get; set; }

        // Жизненный статус животного, доступные значения
        // “ALIVE”(устанавливается автоматически при добавлении нового животного),
        // “DEAD”(можно установить при обновлении информации о животном)
        public LifeStatusType LifeStatus { get; set; }

        // Дата и время чипирования в формате ISO-8601
        // (устанавливается автоматически на момент добавления животного)
        public DateTime ChippingDateTime { get; set; }

        // Идентификатор аккаунта пользователя
        public int ChipperId { get; set; }

        // Идентификатор точки локации животных
        public long ChippingLocationId { get; set; }

        // Массив идентификаторов объектов с информацией о посещенных точках локаций
        public long[] VisitedLocations { get; set; }

        // Дата и время смерти животного в формате ISO-8601
        // (устанавливается автоматически при смене lifeStatus на “DEAD”).
        // Равняется null, пока lifeStatus = “ALIVE”.
        public DateTime? DeathDateTime { get; set; }
    }
}
