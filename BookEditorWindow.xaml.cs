using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Database_Tool
{
    /// <summary>
    /// Логика взаимодействия для BookEditorWindow.xaml
    /// </summary>
    public partial class BookEditorWindow : Window
    {
        private Library _context;
        private Book _book;

        public BookEditorWindow(int? bookId = null)
        {
            InitializeComponent();
            _context = new Library();
            LoadData();

            if (bookId.HasValue)
            {
                _book = _context.Books.Find(bookId.Value);
                if (_book != null)
                {
                    TitleTextBox.Text = _book.Title;
                    PublishYearTextBox.Text = _book.PublishYear.ToString();
                    ISBNTextBox.Text = _book.ISBN;
                    QuantityTextBox.Text = _book.QuantityInStock.ToString();
                    AuthorComboBox.SelectedValue = _book.Author;
                    GenreComboBox.SelectedValue = _book.Genre;
                }
            }
            else
            {
                _book = new Book();
            }
        }

        private void LoadData()
        {
            AuthorComboBox.ItemsSource = _context.Authors
                .Select(a => new { a.ID, FullName = a.FirstName + " " + a.LastName })
                .ToList();
            AuthorComboBox.DisplayMemberPath = "FullName";
            AuthorComboBox.SelectedValuePath = "ID";

            GenreComboBox.ItemsSource = _context.Genres.ToList();
            GenreComboBox.DisplayMemberPath = "Name";
            GenreComboBox.SelectedValuePath = "ID";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(PublishYearTextBox.Text) ||
                string.IsNullOrWhiteSpace(ISBNTextBox.Text) ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                AuthorComboBox.SelectedValue == null ||
                GenreComboBox.SelectedValue == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _book.Title = TitleTextBox.Text;
            _book.PublishYear = int.Parse(PublishYearTextBox.Text);
            _book.ISBN = ISBNTextBox.Text;
            _book.QuantityInStock = int.Parse(QuantityTextBox.Text);
            _book.Author = (int)AuthorComboBox.SelectedValue;
            _book.Genre = (int)GenreComboBox.SelectedValue;

            if (_book.ID == 0)
                _context.Books.Add(_book);

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}
