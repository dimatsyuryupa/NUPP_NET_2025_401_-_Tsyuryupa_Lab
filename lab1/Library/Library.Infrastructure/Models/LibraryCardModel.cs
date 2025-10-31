using System;

namespace Library.Infrastructure.Models
{
    public class LibraryCard
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; } = string.Empty;

        // Один-до-одного: картка -> власник
        public Guid OwnerId { get; set; }
        public Person? Owner { get; set; }

        public DateTime IssuedDate { get; set; } = DateTime.Now;

        // EF constructor
        public LibraryCard() { }

        public LibraryCard(string number, Person owner)
        {
            Number = number;
            Owner = owner;
            OwnerId = owner.Id;
        }
    }
}
