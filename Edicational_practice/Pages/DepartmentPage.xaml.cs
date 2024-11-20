using Edicational_practice.Data;
using Edicational_practice.DbConnection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Edicational_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : Page
    {
        private string userRole;
        private int userEmployeeId;
        public bool IsReadOnly { get; set; }

        public DepartmentPage(string role, int employeeId)
        {
            InitializeComponent();
            userRole = role;
            userEmployeeId = employeeId;
            LoadDepartmentsList();
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
            DepartmentsDataGrid.IsReadOnly = IsReadOnly;
        }

        public void LoadDepartmentsList()
        {
            if (userRole == "зав. кафедрой")
            {
                string departmentCode = DataBaseManager.GetDepartmentCodeByEmployeeId(userEmployeeId);
                var departments = DataBaseManager.GetDepartments().Where(d => d.cipher == departmentCode).ToList();
                DepartmentsDataGrid.ItemsSource = departments;
            }
            else
            {
                var departments = DataBaseManager.GetDepartments();
                DepartmentsDataGrid.ItemsSource = departments;
            }
        }

        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddDepartmentPage(this));
        }

        private void DeleteDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentsDataGrid.SelectedItem is Department selectedDepartment)
            {
                if (DataBaseManager.RemoveDepartments(selectedDepartment))
                {
                    MessageBox.Show("Кафедра успешно удалена!");
                    LoadDepartmentsList();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении кафедры.");
                }
            }
            else
            {
                MessageBox.Show("Выберите кафедру для удаления.");
            }
        }

        private void DepartmentsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedDepartment = e.Row.Item as Department;
                if (editedDepartment != null)
                {
                    if (!DataBaseManager.UpdateDepartment(editedDepartment))
                    {
                        MessageBox.Show("Ошибка при обновлении данных кафедры.");
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;

            foreach (var department in DepartmentsDataGrid.ItemsSource.OfType<Department>())
            {
                if (!DataBaseManager.UpdateDepartment(department))
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                MessageBox.Show("Данные кафедр успешно сохранены!");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных кафедр.");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}