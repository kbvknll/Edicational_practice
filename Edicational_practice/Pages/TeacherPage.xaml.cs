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
    /// Логика взаимодействия для TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private string userRole;
        private int userEmployeeId;
        public bool IsReadOnly { get; set; }
        private List<Employee> originalTeachers;

        public TeacherPage(string role, int employeeId)
        {
            InitializeComponent();
            userRole = role;
            userEmployeeId = employeeId;
            LoadTeachersList();
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
                    AddBt.Visibility = Visibility.Visible;
                    DelBt.Visibility = Visibility.Visible;
                    IsReadOnly = false;
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
            TeachersDataGrid.IsReadOnly = IsReadOnly;
        }

        public void LoadTeachersList()
        {
            if (userRole == "зав. кафедрой")
            {
                string departmentCode = DataBaseManager.GetDepartmentCodeByEmployeeId(userEmployeeId);
                originalTeachers = DataBaseManager.GetEmployees().Where(t => t.post == "преподаватель" && t.cipher == departmentCode).ToList();
            }
            else
            {
                originalTeachers = DataBaseManager.GetEmployees().Where(t => t.post == "преподаватель").ToList();
            }

            FilterTeachers();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterTeachers();
        }

        private void FilterTeachers()
        {
            var filteredTeachers = originalTeachers.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                filteredTeachers = filteredTeachers.Where(t => t.surname.Contains(SearchTextBox.Text));
            }

            TeachersDataGrid.ItemsSource = filteredTeachers.ToList();
        }

        private void AddTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddTeacherPage(this));
        }

        private void DeleteTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeachersDataGrid.SelectedItem is Employee selectedTeacher)
            {
                if (DataBaseManager.RemoveTeachers(selectedTeacher))
                {
                    MessageBox.Show("Учитель успешно удален!");
                    LoadTeachersList();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении учителя.");
                }
            }
            else
            {
                MessageBox.Show("Выберите учителя для удаления.");
            }
        }

        private void TeachersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedTeacher = e.Row.Item as Employee;
                if (editedTeacher != null)
                {
                    if (!DataBaseManager.UpdateTeacher(editedTeacher))
                    {
                        MessageBox.Show("Ошибка при обновлении данных учителя.");
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;

            foreach (var teacher in TeachersDataGrid.ItemsSource.OfType<Employee>())
            {
                if (!DataBaseManager.UpdateTeacher(teacher))
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                MessageBox.Show("Данные учителей успешно сохранены!");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных учителей.");
            }
        }
    }
}