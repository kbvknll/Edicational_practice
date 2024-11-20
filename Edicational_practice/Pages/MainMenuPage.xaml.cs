using System.Windows;
using System.Windows.Controls;
using Edicational_practice.Data;
using Edicational_practice.Pages;

namespace Edicational_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        private string userRole;
        private int userEmployeeId;

        public MainMenuPage(string role, int employeeId)
        {
            InitializeComponent();
            userRole = role;
            userEmployeeId = employeeId;
            UpdateUI();
            SetWelcomeMessage();
        }

        private void UpdateUI()
        {
            switch (userRole)
            {
                case "инженер":
                    SubjectsBtn.Visibility = Visibility.Visible;
                    TeachersBtn.Visibility = Visibility.Visible;
                    ExamsBtn.Visibility = Visibility.Visible;
                    DepartmentsBtn.Visibility = Visibility.Visible;
                    break;
                case "зав. кафедрой":
                    SubjectsBtn.Visibility = Visibility.Visible;
                    TeachersBtn.Visibility = Visibility.Visible;
                    ExamsBtn.Visibility = Visibility.Collapsed;
                    DepartmentsBtn.Visibility = Visibility.Visible;
                    break;
                case "преподаватель":
                    SubjectsBtn.Visibility = Visibility.Visible;
                    TeachersBtn.Visibility = Visibility.Collapsed;
                    ExamsBtn.Visibility = Visibility.Visible;
                    DepartmentsBtn.Visibility = Visibility.Collapsed;
                    break;
                default:
                    SubjectsBtn.Visibility = Visibility.Collapsed;
                    TeachersBtn.Visibility = Visibility.Collapsed;
                    ExamsBtn.Visibility = Visibility.Collapsed;
                    DepartmentsBtn.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void SetWelcomeMessage()
        {
            // Получаем информацию о пользователе по его идентификатору
            var employee = DataBaseManager.GetEmployee(userEmployeeId); // Используем существующий метод
            if (employee != null)
            {
                // Формируем приветствие с фамилией и инициалами
                string welcomeMessage = $"{employee.surname}";
                WelcomeLabel.Content = welcomeMessage;
            }
        }

        private void SubjectsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SubjectPage(userRole));
        }

        private void TeachersBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TeacherPage(userRole, userEmployeeId));
        }

        private void ExamsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ExamPage(userRole));
        }

        private void DepartmentsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DepartmentPage(userRole, userEmployeeId));
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }
    }
}