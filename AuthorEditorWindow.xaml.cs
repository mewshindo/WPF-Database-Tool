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
    /// Логика взаимодействия для AuthorEditorWindow.xaml
    /// </summary>
    public partial class AuthorEditorWindow : Window
    {
        private Library _context;
        private Author _selectedAuthor;

        public AuthorEditorWindow()
        {
            InitializeComponent();
            _context = new Library();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            AuthorsGrid.ItemsSource = _context.Authors.ToList();
        }

        private void AuthorsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Author author)
            {
                _selectedAuthor = author;
                FirstNameTextBox.Text = author.FirstName;
                LastNameTextBox.Text = author.LastName;
                BirthDatePicker.SelectedDate = author.BirthDate;
                CountryTextBox.Text = author.Country;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAuthor == null)
                return;

            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountryTextBox.Text) ||
                BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedAuthor.FirstName = FirstNameTextBox.Text;
            _selectedAuthor.LastName = LastNameTextBox.Text;
            _selectedAuthor.BirthDate = BirthDatePicker.SelectedDate.Value;
            _selectedAuthor.Country = CountryTextBox.Text;

            _context.SaveChanges();
            LoadAuthors();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstNameTextBox.Text == "" || LastNameTextBox.Text == "" || CountryTextBox.Text == "" || BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                var newAuthor = new Author
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    BirthDate = DateTime.SpecifyKind(BirthDatePicker.SelectedDate.Value, DateTimeKind.Utc),
                    Country = CountryTextBox.Text
                };
                _context.Authors.Add(newAuthor);
                _context.SaveChanges();
                LoadAuthors();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAuthor == null)
                return;

            if (MessageBox.Show($"Удалить автора {_selectedAuthor.FirstName} {_selectedAuthor.LastName}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _context.Authors.Remove(_selectedAuthor);
                _context.SaveChanges();
                LoadAuthors();
            }
        }
    }
}
