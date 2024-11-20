using Edicational_practice.Data;
using Edicational_practice.DbConnection;
using System.Windows;
using System.Windows.Controls;

namespace Edicational_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddDepartmentPage.xaml
    /// </summary>
    public partial class AddDepartmentPage : Page
    {
        private DepartmentPage _departmentPage;

        public AddDepartmentPage(DepartmentPage departmentPage)
        {
            InitializeComponent();
            _departmentPage = departmentPage;
            LoadFaculties();
        }

        private void LoadFaculties()
        {
            var faculties = DataBaseManager.GetFaculties();
            FacultyCB.ItemsSource = faculties;
            FacultyCB.DisplayMemberPath = "name";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string cipher = CipherTextBox.Text.Trim();
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(cipher) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var selectedFaculty = (Faculty)FacultyCB.SelectedItem;

            if (selectedFaculty == null)
            {
                MessageBox.Show("Пожалуйста, выберите факультет.");
                return;
            }

            var newDepartment = new Department
            {
                cipher = cipher,
                name = name,
                Faculty = selectedFaculty
            };

            if (DataBaseManager.AddDepartments(newDepartment))
            {
                DataBaseManager.UpdateDataBase();
                MessageBox.Show("Кафедра успешно добавлена!");
                _departmentPage.LoadDepartmentsList();
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении кафедры.");
            }
        }
    }
}