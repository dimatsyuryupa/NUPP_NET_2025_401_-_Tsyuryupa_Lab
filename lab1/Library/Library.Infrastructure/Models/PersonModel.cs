using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Infrastructure.Models
{
    public abstract class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // тепер простий числовий Id

        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }

        // Навігаційні властивості
        public virtual List<LibraryCard> LibraryCards { get; set; } = new();

        // EF constructor
        protected Person() { }

        protected Person(string fullName, int age)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Age = age;
        }

        public abstract void ShowInfo();
    }
}
