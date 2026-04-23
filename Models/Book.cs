// 
// Файл: Models/Book.cs
// Назначение: класс-модель книги библиотеки
// Зависимости: нет (базовый класс)
// 
using System;
namespace LibraryManager.Models
{
    /// <summary>
    /// Представляет книгу в каталоге библиотеки.
    /// </summary>
    public class Book
    {
        public int Id { get; set; } // уникальный ID
        public string Title { get; set; } // название
        public string Author { get; set; } // автор
        public int Year { get; set; } // год издания
        public string ISBN { get; set; } // ISBN
        public int TotalCopies { get; set; } // всего экземпляров
        public int AvailableCopies { get; set; } // доступно
        public string Genre { get; set; } // жанр
        public override string ToString() =>
        $"{Id} | {Title} | {Author} | {Year} | Доступно: {AvailableCopies}/{TotalCopies}";
    }
}