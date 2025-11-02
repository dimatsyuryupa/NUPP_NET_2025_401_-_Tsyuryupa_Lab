using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Infrastructure.Models
{
    public class LibraryCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Тепер простий числовий Id

        public string Number { get; set; } = string.Empty;

        // Один-до-одного: картка -> власник
        public int OwnerId { get; set; } // якщо Person теж int
        public Person? Owner { get; set; }

        public DateTime IssuedDate { get; set; } = DateTime.Now;

        // Parameterless ctor для EF Core
        protected LibraryCard() { }

        // Конструктор для EF Core
        public LibraryCard(string number, int ownerId)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
            OwnerId = ownerId;
        }

        // Зручний конструктор для застосунку
        public LibraryCard(string number, Person owner)
            : this(number, owner?.Id ?? throw new ArgumentNullException(nameof(owner)))
        {
            Owner = owner;
        }

        // Для відображення інформації
        public void ShowInfo() =>
            Console.WriteLine($"Картка №{Number}, Власник: {Owner?.FullName ?? "—"}, Видана: {IssuedDate:d}");
    }
}
