using System.Collections.Generic;
using System.Windows;
using LibraryManager.Business;
using LibraryManager.Models;

namespace LibraryManager
{
    public partial class MainWindow : Window
    {
        private LibraryCore libraryCore;
        private SearchService searchService;

        public MainWindow()
        {
            InitializeComponent();
            libraryCore = new LibraryCore();
            searchService = new SearchService(libraryCore);
        }

        private void BtnInit_Click(object sender, RoutedEventArgs e)
        {
            libraryCore.InitializeLibrary();
            RefreshBooksGrid();
            MessageBox.Show("Библиотека инициализирована");
        }

        private void BtnAddBook_Click(object sender, RoutedEventArgs e)
        {
            int newBookId;
            bool result = libraryCore.AddBook(
                "Новая книга",
                "Неизвестный автор",
                2024,
                "978-5-17-123456-7",
                1,
                "Жанр",
                out newBookId);

            if (result)
            {
                RefreshBooksGrid();
                MessageBox.Show("Книга добавлена");
            }
            else
            {
                MessageBox.Show("Не удалось добавить книгу");
            }
        }

        private void BtnAddReader_Click(object sender, RoutedEventArgs e)
        {
            int newReaderId;
            bool result = libraryCore.RegisterReader(
                "Иванов",
                "Иван",
                "Иванович",
                "+7-900-123-45-67",
                "ivanov@email.com",
                out newReaderId);

            if (result)
            {
                MessageBox.Show("Читатель добавлен");
            }
            else
            {
                MessageBox.Show("Не удалось добавить читателя");
            }
        }

        private void BtnIssueBook_Click(object sender, RoutedEventArgs e)
        {
            int newLoanId;
            bool result = libraryCore.IssueBook(1, 101, 14, out newLoanId);

            if (result)
            {
                RefreshBooksGrid();
                MessageBox.Show("Книга выдана");
            }
            else
            {
                MessageBox.Show("Не удалось выдать книгу");
            }
        }

        private void BtnReturnBook_Click(object sender, RoutedEventArgs e)
        {
            bool result = libraryCore.ReturnBook(1001);

            if (result)
            {
                RefreshBooksGrid();
                MessageBox.Show("Книга возвращена");
            }
            else
            {
                MessageBox.Show("Не удалось вернуть книгу");
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Book> result = searchService.SearchByTitle(TbSearch.Text);
            BooksGrid.ItemsSource = null;
            BooksGrid.ItemsSource = result;
        }

        private void RefreshBooksGrid()
        {
            BooksGrid.ItemsSource = null;
            BooksGrid.ItemsSource = libraryCore.GetAllBooks();
        }
    }
}