using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Edicational_practice.Pages;

namespace Edicational_practice
{
    public partial class MainWindow : Window
    {
        private Dictionary<int, string> employees = new Dictionary<int, string>
        {
            { 101, "зав. кафедрой" },
            { 102, "преподаватель" },
            { 105, "преподаватель" },
            { 153, "инженер" },
            { 201, "зав. кафедрой" },
            { 202, "преподаватель" },
            { 241, "инженер" },
            { 242, "инженер" },
            { 301, "зав. кафедрой" },
            { 302, "преподаватель" },
            { 401, "зав. кафедрой" },
            { 402, "преподаватель" },
            { 403, "преподаватель" },
            { 435, "инженер" },
            { 501, "зав. кафедрой" },
            { 502, "преподаватель" },
            { 503, "преподаватель" },
            { 601, "зав. кафедрой" },
            { 602, "преподаватель" },
            { 614, "инженер" }
        };

        private string userRole;
        private int userEmployeeId;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigated += MainFrame_Navigated; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new QrPage());
            BackBtn.Visibility = Visibility.Visible;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            BackBtn.Visibility = MainFrame.NavigationService.CanGoBack ? Visibility.Visible : Visibility.Collapsed;

            if (e.Content is QrPage || e.Content is AddTeacherPage || e.Content is AddDepartmentPage || e.Content is AddExamPage || e.Content is AddSubjectPage || e.Content is MainMenuPage)
            {
                BackBtn.Visibility = Visibility.Visible;
                QrBtn.Visibility = Visibility.Collapsed;
                PasswordTb.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                AuthBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                QrBtn.Visibility = Visibility.Visible;
                PasswordTb.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Visible;
                AuthBtn.Visibility = Visibility.Visible;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoBack)
            {
                MainFrame.NavigationService.GoBack();
            }
            else
            {
                MainFrame.NavigationService.Navigate(new MainMenuPage(userRole, userEmployeeId));
            }
        }

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(PasswordTextBox.Password, out int idEmployee))
            {
                if (employees.TryGetValue(idEmployee, out string post))
                {
                    userRole = post;
                    userEmployeeId = idEmployee;
                    switch (post)
                    {
                        case "инженер":
                            MessageBox.Show("Здравствуй, инженер!");
                            break;
                        case "преподаватель":
                            MessageBox.Show("Здравствуй, преподаватель!");
                            break;
                        default:
                            MessageBox.Show("Здравствуй, зав. кафедрой!");
                            break;
                    }
                    MainFrame.NavigationService.Navigate(new MainMenuPage(userRole, userEmployeeId));
                    PasswordTb.Visibility = Visibility.Collapsed;
                    PasswordTextBox.Visibility = Visibility.Collapsed;
                    AuthBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Сотрудник с таким ID не найден.");
                }
            }
            else
            {
                MessageBox.Show("Введите корректный ID сотрудника.");
            }
        }
    }
}