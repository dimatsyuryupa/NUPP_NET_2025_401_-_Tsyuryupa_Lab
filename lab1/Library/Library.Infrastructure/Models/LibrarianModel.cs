using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Infrastructure.Models
{
    public class Librarian
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Тепер простий числовий Id

        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Position { get; set; } = string.Empty;
        public int Experience { get; set; }

        // Parameterless ctor для EF Core
        protected Librarian() { }

        // Конструктор для створення в коді
        public Librarian(string fullName, int age, string position, int experience)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Age = age;
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Experience = experience;
        }

        // Для зручного відображення інформації
        public void ShowInfo() =>
            Console.WriteLine($"Бібліотекар: {FullName}, Вік: {Age}, Посада: {Position}, Досвід: {Experience} років");
    }
}
