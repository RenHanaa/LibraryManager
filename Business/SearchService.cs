using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManager.Models;

namespace LibraryManager.Business
{
    public class SearchService
    {
        private readonly LibraryCore libraryCore;

        public SearchService(LibraryCore libraryCore)
        {
            this.libraryCore = libraryCore;
        }

        public List<Book> SearchByTitle(string query)
        {
            List<Book> allBooks = libraryCore.GetAllBooks();
            List<Book> result = new List<Book>();
            string normalizedQuery = NormalizeString(query);

            foreach (Book book in allBooks)
            {
                string normalizedTitle = NormalizeString(book.Title);

                if (normalizedTitle.Contains(normalizedQuery))
                    result.Add(book);
            }

            return result;
        }

        public List<Book> SearchByAuthor(string author)
        {
            List<Book> allBooks = libraryCore.GetAllBooks();
            List<Book> result = new List<Book>();
            string normalizedAuthor = NormalizeString(author);

            foreach (Book book in allBooks)
            {
                string currentAuthor = NormalizeString(book.Author);

                if (currentAuthor.Contains(normalizedAuthor))
                    result.Add(book);
            }

            result = result.OrderBy(book => book.Year).ToList();
            return result;
        }

        public List<Book> SearchByGenre(string genre)
        {
            List<Book> allBooks = libraryCore.GetAllBooks();
            List<Book> result = new List<Book>();

            foreach (Book book in allBooks)
            {
                if (string.Equals(book.Genre, genre, StringComparison.OrdinalIgnoreCase))
                    result.Add(book);
            }

            return result;
        }

        public List<Book> SearchByYear(int yearFrom, int yearTo)
        {
            List<Book> allBooks = libraryCore.GetAllBooks();
            List<Book> result = new List<Book>();

            foreach (Book book in allBooks)
            {
                if (book.Year >= yearFrom && book.Year <= yearTo)
                    result.Add(book);
            }

            result = result.OrderBy(book => book.Year).ToList();
            return result;
        }

        public Book SearchByIsbn(string isbn)
        {
            List<Book> allBooks = libraryCore.GetAllBooks();

            foreach (Book book in allBooks)
            {
                if (book.ISBN == isbn)
                    return book;
            }

            return null;
        }

        public List<Book> AdvancedSearch(string title, string author, string genre, int? year)
        {
            List<Book> result = libraryCore.GetAllBooks();

            if (!string.IsNullOrWhiteSpace(title))
            {
                string normalizedTitle = NormalizeString(title);
                result = result
                    .Where(book => NormalizeString(book.Title).Contains(normalizedTitle))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                string normalizedAuthor = NormalizeString(author);
                result = result
                    .Where(book => NormalizeString(book.Author).Contains(normalizedAuthor))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                result = result
                    .Where(book => string.Equals(book.Genre, genre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (year.HasValue)
            {
                result = result
                    .Where(book => book.Year == year.Value)
                    .ToList();
            }

            return result;
        }

        private string NormalizeString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Trim().ToLower();
        }
    }
}