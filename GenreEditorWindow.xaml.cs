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
    /// Логика взаимодействия для GenreEditorWindow.xaml
    /// </summary>
    public partial class GenreEditorWindow : Window
    {
        private Library _context;
        private Genre _selectedGenre;

        public GenreEditorWindow()
        {
            InitializeComponent();
            _context = new Library();
            LoadGenres();
        }

        private void LoadGenres()
        {
            GenresGrid.ItemsSource = _context.Genres.ToList();
        }

        private void GenresGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre genre)
            {
                _selectedGenre = genre;
                NameTextBox.Text = genre.Name;
                DescriptionTextBox.Text = genre.Description;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGenre == null)
                return;

            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Название жанра не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedGenre.Name = NameTextBox.Text;
            _selectedGenre.Description = DescriptionTextBox.Text;

            _context.SaveChanges();
            LoadGenres();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newGenre = new Genre
            {
                Name = "",
                Description = ""
            };
            _context.Genres.Add(newGenre);
            _context.SaveChanges();
            LoadGenres();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGenre == null)
                return;

            if (MessageBox.Show($"Удалить жанр {_selectedGenre.Name}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _context.Genres.Remove(_selectedGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
    }
}
