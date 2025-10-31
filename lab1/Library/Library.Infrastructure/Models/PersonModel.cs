using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Models
{
    public abstract class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }

        // Навігаційні властивості для зв’язків
        public List<LibraryCard> LibraryCards { get; set; } = new();

        // EF constructor
        protected Person() { }

        protected Person(string fullName, int age)
        {
            FullName = fullName;
            Age = age;
        }

        public abstract void ShowInfo();
    }
}
