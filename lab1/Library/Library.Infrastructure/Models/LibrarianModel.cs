using System;

namespace Library.Infrastructure.Models
{
    public class Librarian
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Position { get; set; } = string.Empty;
        public int Experience { get; set; }

        public Librarian() { }

        public Librarian(string fullName, int age, string position, int experience)
        {
            FullName = fullName;
            Age = age;
            Position = position;
            Experience = experience;
        }
    }
}
