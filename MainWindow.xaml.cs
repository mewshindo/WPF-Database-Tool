using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Database_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Library _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new Library();
            LoadData();
        }

        private void LoadData()
        {
            // Загружаем книги, авторов и жанры
            BooksGrid.ItemsSource = _context.Books
                .Select(b => new
                {
                    b.ID,
                    b.Title,
                    Author = b.AuthorClass.FirstName + " " + b.AuthorClass.LastName,
                    Genre = b.GenreClass.Name,
                    b.PublishYear,
                    b.ISBN,
                    b.QuantityInStock
                }).ToList();

            // Заполняем фильтры
            GenreFilter.ItemsSource = _context.Genres.ToList();
            GenreFilter.DisplayMemberPath = "Name";
            GenreFilter.SelectedValuePath = "ID";

            AuthorFilter.ItemsSource = _context.Authors
                .Select(a => new { a.ID, FullName = $"{a.FirstName} {a.LastName}" })
                .ToList();
            AuthorFilter.DisplayMemberPath = "FullName";
            AuthorFilter.SelectedValuePath = "ID";
        }

        private void GenreFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void AuthorFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var books = _context.Books.AsQueryable();

            if (GenreFilter.SelectedValue != null)
            {
                int genreId = (int)GenreFilter.SelectedValue;
                books = books.Where(b => b.Genre == genreId);
            }

            if (AuthorFilter.SelectedValue != null)
            {
                int authorId = (int)AuthorFilter.SelectedValue;
                books = books.Where(b => b.Author == authorId);
            }

            BooksGrid.ItemsSource = books
            .Select(b => new
            {
                b.ID,
                b.Title,
                Author = $"{b.AuthorClass.FirstName} {b.AuthorClass.LastName}",
                Genre = b.GenreClass.Name,
                b.PublishYear,
                b.ISBN,
                b.QuantityInStock
            }).ToList();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            GenreFilter.SelectedIndex = -1;
            AuthorFilter.SelectedIndex = -1;
            LoadData();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var window = new BookEditorWindow();
            window.ShowDialog();
            LoadData();
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is null) return;

            var selectedBook = (BooksGrid.SelectedItem as dynamic);
            if (selectedBook == null) return;

            int bookId = selectedBook.ID;
            var window = new BookEditorWindow(bookId);
            window.ShowDialog();
            LoadData();
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is null) return;

            var selectedBook = (BooksGrid.SelectedItem as dynamic);
            if (selectedBook == null) return;

            int bookId = selectedBook.ID;
            var book = _context.Books.Find(bookId);

            if (book != null)
            {
                if (MessageBox.Show($"Удалить книгу \"{book.Title}\"?",
                                    "Подтверждение",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void AuthorsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthorEditorWindow();
            window.ShowDialog();
            LoadData();
        }
        private void GenresButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new GenreEditorWindow();
            window.ShowDialog();
            LoadData();
        }
    }
}