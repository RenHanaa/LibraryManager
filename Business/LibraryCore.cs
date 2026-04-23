using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.Business
{
    public class LibraryCore
    {
        private List<Book> books;
        private List<Reader> readers;
        private List<Loan> loans;

        private int lastBookId = 0;
        private int lastReaderId = 100;
        private int lastLoanId = 1000;

        private readonly StorageService storageService;

        public LibraryCore()
        {
            books = new List<Book>();
            readers = new List<Reader>();
            loans = new List<Loan>();
            storageService = new StorageService();
        }

        public bool InitializeLibrary()
        {
            books = storageService.LoadBooks();
            readers = storageService.LoadReaders();
            loans = storageService.LoadLoans();

            if (books.Count > 0)
                lastBookId = books.Max(b => b.Id);

            if (readers.Count > 0)
                lastReaderId = readers.Max(r => r.Id);

            if (loans.Count > 0)
                lastLoanId = loans.Max(l => l.Id);

            return true;
        }

        public bool AddBook(string title, string author, int year, string isbn, int totalCopies, string genre, out int newBookId)
        {
            newBookId = 0;

            string errorMessage;
            if (!ValidationHelper.ValidateBook(title, author, year, isbn, totalCopies, genre, out errorMessage))
                return false;

            lastBookId++;

            Book book = new Book
            {
                Id = lastBookId,
                Title = title,
                Author = author,
                Year = year,
                ISBN = isbn,
                TotalCopies = totalCopies,
                AvailableCopies = totalCopies,
                Genre = genre
            };

            books.Add(book);

            if (!storageService.SaveBooks(books))
                return false;

            newBookId = book.Id;
            return true;
        }

        public List<Book> GetAllBooks()
        {
            return books;
        }

        public Book FindBookById(int bookId)
        {
            foreach (Book book in books)
            {
                if (book.Id == bookId)
                    return book;
            }

            return null;
        }

        public bool RegisterReader(string lastName, string firstName, string middleName, string phone, string email, out int newReaderId)
        {
            newReaderId = 0;

            string errorMessage;
            if (!ValidationHelper.ValidateReader(lastName, firstName, middleName, phone, email, out errorMessage))
                return false;

            lastReaderId++;

            Reader reader = new Reader
            {
                Id = lastReaderId,
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
                RegistrationDate = DateTime.Now,
                Phone = phone,
                Email = email
            };

            readers.Add(reader);

            if (!storageService.SaveReaders(readers))
                return false;

            newReaderId = reader.Id;
            return true;
        }

        public List<Reader> GetAllReaders()
        {
            return readers;
        }

        public Reader FindReaderById(int readerId)
        {
            foreach (Reader reader in readers)
            {
                if (reader.Id == readerId)
                    return reader;
            }

            return null;
        }

        public bool IssueBook(int bookId, int readerId, int loanDays, out int newLoanId)
        {
            newLoanId = 0;

            Book book = FindBookById(bookId);
            if (book == null)
                return false;

            if (book.AvailableCopies <= 0)
                return false;

            Reader reader = FindReaderById(readerId);
            if (reader == null)
                return false;

            List<Loan> activeLoans = GetActiveLoansByReader(readerId);
            if (activeLoans.Count >= 5)
                return false;

            book.AvailableCopies--;

            lastLoanId++;

            Loan loan = new Loan
            {
                Id = lastLoanId,
                BookId = bookId,
                ReaderId = readerId,
                IssuedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(loanDays),
                Status = LoanStatus.Issued
            };

            loans.Add(loan);

            bool booksSaved = storageService.SaveBooks(books);
            bool loansSaved = storageService.SaveLoans(loans);

            if (!booksSaved || !loansSaved)
                return false;

            newLoanId = loan.Id;
            return true;
        }

        public bool ReturnBook(int loanId)
        {
            Loan loan = null;

            foreach (Loan currentLoan in loans)
            {
                if (currentLoan.Id == loanId)
                {
                    loan = currentLoan;
                    break;
                }
            }

            if (loan == null)
                return false;

            if (loan.Status == LoanStatus.Returned)
                return false;

            Book book = FindBookById(loan.BookId);
            if (book == null)
                return false;

            book.AvailableCopies++;
            loan.Status = LoanStatus.Returned;

            bool booksSaved = storageService.SaveBooks(books);
            bool loansSaved = storageService.SaveLoans(loans);

            if (!booksSaved || !loansSaved)
                return false;

            return true;
        }

        public List<Loan> GetActiveLoansByReader(int readerId)
        {
            List<Loan> result = new List<Loan>();

            foreach (Loan loan in loans)
            {
                if (loan.ReaderId == readerId && loan.Status == LoanStatus.Issued)
                    result.Add(loan);
            }

            return result;
        }

        public int CheckBookAvailability(int bookId)
        {
            Book book = FindBookById(bookId);

            if (book == null)
                return 0;

            return book.AvailableCopies;
        }
    }
}