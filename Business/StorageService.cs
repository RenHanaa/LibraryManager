using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using LibraryManager.Models;

namespace LibraryManager.Business
{
    public class StorageService
    {
        private const string BooksFilePath = "../data/books.txt";
        private const string ReadersFilePath = "../data/readers.txt";
        private const string LoansFilePath = "../data/loans.txt";
        private const string BackupFolderPath = "../backup";
        private const string DateFormat = "dd.MM.yyyy";

        public bool SaveBooks(List<Book> books)
        {
            try
            {
                EnsureDirectoryExists(BooksFilePath);

                using (StreamWriter writer = new StreamWriter(BooksFilePath, false))
                {
                    writer.WriteLine("ID|Название|Автор|Год|ISBN|Всего|Доступно|Жанр");

                    foreach (Book book in books)
                    {
                        writer.WriteLine($"{book.Id}|{book.Title}|{book.Author}|{book.Year}|{book.ISBN}|{book.TotalCopies}|{book.AvailableCopies}|{book.Genre}");
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Book> LoadBooks()
        {
            List<Book> books = new List<Book>();

            try
            {
                if (!File.Exists(BooksFilePath))
                {
                    EnsureDirectoryExists(BooksFilePath);
                    using (StreamWriter writer = new StreamWriter(BooksFilePath, false))
                    {
                        writer.WriteLine("ID|Название|Автор|Год|ISBN|Всего|Доступно|Жанр");
                    }
                    return books;
                }

                using (StreamReader reader = new StreamReader(BooksFilePath))
                {
                    bool isFirstLine = true;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split('|');
                        if (parts.Length != 8)
                            continue;

                        Book book = new Book
                        {
                            Id = int.Parse(parts[0]),
                            Title = parts[1],
                            Author = parts[2],
                            Year = int.Parse(parts[3]),
                            ISBN = parts[4],
                            TotalCopies = int.Parse(parts[5]),
                            AvailableCopies = int.Parse(parts[6]),
                            Genre = parts[7]
                        };

                        books.Add(book);
                    }
                }
            }
            catch
            {
                return books;
            }

            return books;
        }

        public bool SaveReaders(List<Reader> readers)
        {
            try
            {
                EnsureDirectoryExists(ReadersFilePath);

                using (StreamWriter writer = new StreamWriter(ReadersFilePath, false))
                {
                    writer.WriteLine("ID|Фамилия|Имя|Отчество|ДатаРегистрации|Телефон|Email");

                    foreach (Reader reader in readers)
                    {
                        writer.WriteLine($"{reader.Id}|{reader.LastName}|{reader.FirstName}|{reader.MiddleName}|{reader.RegistrationDate.ToString(DateFormat)}|{reader.Phone}|{reader.Email}");
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Reader> LoadReaders()
        {
            List<Reader> readers = new List<Reader>();

            try
            {
                if (!File.Exists(ReadersFilePath))
                {
                    EnsureDirectoryExists(ReadersFilePath);
                    using (StreamWriter writer = new StreamWriter(ReadersFilePath, false))
                    {
                        writer.WriteLine("ID|Фамилия|Имя|Отчество|ДатаРегистрации|Телефон|Email");
                    }
                    return readers;
                }

                using (StreamReader reader = new StreamReader(ReadersFilePath))
                {
                    bool isFirstLine = true;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split('|');
                        if (parts.Length != 7)
                            continue;

                        Reader newReader = new Reader
                        {
                            Id = int.Parse(parts[0]),
                            LastName = parts[1],
                            FirstName = parts[2],
                            MiddleName = parts[3],
                            RegistrationDate = DateTime.ParseExact(parts[4], DateFormat, CultureInfo.InvariantCulture),
                            Phone = parts[5],
                            Email = parts[6]
                        };

                        readers.Add(newReader);
                    }
                }
            }
            catch
            {
                return readers;
            }

            return readers;
        }

        public bool SaveLoans(List<Loan> loans)
        {
            try
            {
                EnsureDirectoryExists(LoansFilePath);

                using (StreamWriter writer = new StreamWriter(LoansFilePath, false))
                {
                    writer.WriteLine("ID|IDКниги|IDЧитателя|ДатаВыдачи|ДатаВозврата|Статус");

                    foreach (Loan loan in loans)
                    {
                        string status = loan.Status == LoanStatus.Issued ? "Выдана" : "Возвращена";
                        writer.WriteLine($"{loan.Id}|{loan.BookId}|{loan.ReaderId}|{loan.IssuedDate.ToString(DateFormat)}|{loan.DueDate.ToString(DateFormat)}|{status}");
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Loan> LoadLoans()
        {
            List<Loan> loans = new List<Loan>();

            try
            {
                if (!File.Exists(LoansFilePath))
                {
                    EnsureDirectoryExists(LoansFilePath);
                    using (StreamWriter writer = new StreamWriter(LoansFilePath, false))
                    {
                        writer.WriteLine("ID|IDКниги|IDЧитателя|ДатаВыдачи|ДатаВозврата|Статус");
                    }
                    return loans;
                }

                using (StreamReader reader = new StreamReader(LoansFilePath))
                {
                    bool isFirstLine = true;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split('|');
                        if (parts.Length != 6)
                            continue;

                        LoanStatus status = parts[5] == "Выдана"
                            ? LoanStatus.Issued
                            : LoanStatus.Returned;

                        Loan loan = new Loan
                        {
                            Id = int.Parse(parts[0]),
                            BookId = int.Parse(parts[1]),
                            ReaderId = int.Parse(parts[2]),
                            IssuedDate = DateTime.ParseExact(parts[3], DateFormat, CultureInfo.InvariantCulture),
                            DueDate = DateTime.ParseExact(parts[4], DateFormat, CultureInfo.InvariantCulture),
                            Status = status
                        };

                        loans.Add(loan);
                    }
                }
            }
            catch
            {
                return loans;
            }

            return loans;
        }

        public bool CreateBackup()
        {
            try
            {
                if (!Directory.Exists(BackupFolderPath))
                    Directory.CreateDirectory(BackupFolderPath);

                string dateSuffix = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                if (File.Exists(BooksFilePath))
                    File.Copy(BooksFilePath, Path.Combine(BackupFolderPath, $"books_{dateSuffix}.txt"), true);

                if (File.Exists(ReadersFilePath))
                    File.Copy(ReadersFilePath, Path.Combine(BackupFolderPath, $"readers_{dateSuffix}.txt"), true);

                if (File.Exists(LoansFilePath))
                    File.Copy(LoansFilePath, Path.Combine(BackupFolderPath, $"loans_{dateSuffix}.txt"), true);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void EnsureDirectoryExists(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}