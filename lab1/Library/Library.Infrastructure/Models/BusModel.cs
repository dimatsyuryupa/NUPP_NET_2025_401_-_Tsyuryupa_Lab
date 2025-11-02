using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Infrastructure.Models
{
    public class Bus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Тепер простий числовий Id

        public string Model { get; set; } = string.Empty;
        public int Seats { get; set; }
        public int Speed { get; set; }

        private static readonly Random rnd = new();

        // Parameterless ctor для EF Core
        protected Bus() { }

        // Конструктор зі значеннями
        public Bus(string model, int seats, int speed)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Seats = seats;
            Speed = speed;
        }

        // Генерація нового автобуса
        public static Bus CreateNew() => new Bus(
            $"Bus-{rnd.Next(1000, 9999)}",
            rnd.Next(20, 60),
            rnd.Next(60, 120)
        );

        // Для зручного відображення інформації
        public void ShowInfo() =>
            Console.WriteLine($"Автобус: {Model}, Місць: {Seats}, Швидкість: {Speed} км/год");
    }
}
