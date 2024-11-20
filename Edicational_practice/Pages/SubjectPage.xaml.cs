using Edicational_practice.Data;
using Edicational_practice.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Edicational_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для SubjectPage.xaml
    /// </summary>
    public partial class SubjectPage : Page
    {
        private string userRole;
        public bool IsReadOnly { get; set; }

        public SubjectPage(string role)
        {
            InitializeComponent();
            userRole = role;
            LoadSubjectsList();
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (userRole)
            {
                case "инженер":
                    AddBt.Visibility = Visibility.Visible;
                    DelBt.Visibility = Visibility.Visible;
                    IsReadOnly = false;
                    break;
                case "зав. кафедрой":
                    AddBt.Visibility = Visibility.Collapsed;
                    DelBt.Visibility = Visibility.Collapsed;
                    SaveBt.Visibility = Visibility.Collapsed;
                    IsReadOnly = true;
                    break;
                case "преподаватель":
                    AddBt.Visibility = Visibility.Collapsed;
                    DelBt.Visibility = Visibility.Collapsed;
                    SaveBt.Visibility = Visibility.Collapsed;
                    IsReadOnly = true;
                    break;
                default:
                    AddBt.Visibility = Visibility.Collapsed;
                    DelBt.Visibility = Visibility.Collapsed;
                    SaveBt.Visibility = Visibility.Collapsed;
                    IsReadOnly = true;
                    break;
            }

            SubjectsDataGrid.IsReadOnly = IsReadOnly;
        }

        public void LoadSubjectsList()
        {
            var subjects = DataBaseManager.GetDisciplines();
            SubjectsDataGrid.ItemsSource = subjects;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void AddSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddSubjectPage(this));
        }

        private void DeleteSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectsDataGrid.SelectedItem is Discipline selectedSubject)
            {
                if (DataBaseManager.RemoveSubjects(selectedSubject))
                {
                    MessageBox.Show("Предмет успешно удален!");
                    LoadSubjectsList();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении предмета.");
                }
            }
            else
            {
                MessageBox.Show("Выберите предмет для удаления.");
            }
        }

        private void SubjectsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedSubject = e.Row.Item as Discipline;
                if (editedSubject != null)
                {
                    if (!DataBaseManager.UpdateDiscipline(editedSubject))
                    {
                        MessageBox.Show("Ошибка при обновлении данных предмета.");
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;

            foreach (var subject in SubjectsDataGrid.ItemsSource.OfType<Discipline>())
            {
                if (!DataBaseManager.UpdateDiscipline(subject))
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                MessageBox.Show("Данные предметов успешно сохранены!");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных предметов.");
            }
        }
    }
}