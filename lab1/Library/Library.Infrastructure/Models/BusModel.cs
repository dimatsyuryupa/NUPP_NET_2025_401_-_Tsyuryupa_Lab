using System;

namespace Library.Infrastructure.Models
{
    public class Bus
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Model { get; set; } = string.Empty;
        public int Seats { get; set; }
        public int Speed { get; set; }

        private static readonly Random rnd = new();

        public Bus() { }

        public static Bus CreateNew() => new Bus
        {
            Model = $"Bus-{rnd.Next(1000, 9999)}",
            Seats = rnd.Next(20, 60),
            Speed = rnd.Next(60, 120)
        };
    }
}
