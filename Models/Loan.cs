using System;
namespace LibraryManager.Models
{
    public enum LoanStatus { Issued, Returned }
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime DueDate { get; set; }
        public LoanStatus Status { get; set; }
    }
}