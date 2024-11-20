using Edicational_practice.Data;
using Edicational_practice.DbConnection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Edicational_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddTeacherPage.xaml
    /// </summary>
    public partial class AddTeacherPage : Page
    {
        private TeacherPage _teacherPage;

        public AddTeacherPage(TeacherPage teacherPage)
        {
            InitializeComponent();
            _teacherPage = teacherPage;
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            var departments = DataBaseManager.GetDepartments();
            DepartmentCB.ItemsSource = departments;
            DepartmentCB.DisplayMemberPath = "name";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PostTextBox.Text = "преподаватель";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string idTeacher = IdTeacherTextBox.Text.Trim();
            string surname = SurnameTextBox.Text.Trim();
            string post = PostTextBox.Text.Trim();
            string salary = SalaryTextBox.Text.Trim();

            if (string.IsNullOrEmpty(idTeacher) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(post) || string.IsNullOrEmpty(salary))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!int.TryParse(idTeacher, out int idTeacherInt))
            {
                MessageBox.Show("Идентификатор должен быть числом.");
                return;
            }

            if (!int.TryParse(salary, out int salaryInt))
            {
                MessageBox.Show("Зарплата должна быть числом.");
                return;
            }

            var selectedDepartment = (Department)DepartmentCB.SelectedItem;

            if (selectedDepartment == null)
            {
                MessageBox.Show("Пожалуйста, выберите кафедру.");
                return;
            }

            var newTeacher = new Employee
            {
                id_employee = idTeacherInt,
                surname = surname,
                post = post,
                salary = salaryInt,
                cipher = selectedDepartment.cipher
            };

            if (DataBaseManager.AddTeachers(newTeacher))
            {
                DataBaseManager.UpdateDataBase();
                MessageBox.Show("Учитель успешно добавлен!");
                _teacherPage.LoadTeachersList();
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении учителя.");
            }
        }
    }
}