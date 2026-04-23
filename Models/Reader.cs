using System;
namespace LibraryManager.Models
{
    public class Reader
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FullName => $"{LastName} {FirstName} {MiddleName}";
    }
}