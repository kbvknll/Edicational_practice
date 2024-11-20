using Edicational_practice.Data;
using Edicational_practice.DbConnection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Edicational_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddSubjectPage.xaml
    /// </summary>
    public partial class AddSubjectPage : Page
    {
        private SubjectPage _subjectPage;

        public AddSubjectPage(SubjectPage subjectPage)
        {
            InitializeComponent();
            _subjectPage = subjectPage;
            LoadExecutors(); // Загрузка преподавателей для ExecutorCB
        }

        private void LoadExecutors()
        {
            var employees = DataBaseManager.GetEmployees();
            // Фильтруем список сотрудников, чтобы оставить только тех, у кого post = "преподаватель"
            var teachers = employees.Where(e => e.post == "преподаватель").ToList();
            ExecutorCB.ItemsSource = teachers;
            ExecutorCB.DisplayMemberPath = "surname";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string code_discipline = CodeSubjectTextBox.Text.Trim();
            string volume = VolumeTextBox.Text.Trim();
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(code_discipline) || string.IsNullOrEmpty(volume) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!int.TryParse(code_discipline, out int code_disciplineInt))
            {
                MessageBox.Show("Код должен быть числом.");
                return;
            }

            if (!int.TryParse(volume, out int volumeInt))
            {
                MessageBox.Show("Объем должен быть числом.");
                return;
            }

            var selectedExecutor = (Employee)ExecutorCB.SelectedItem;

            if (selectedExecutor == null)
            {
                MessageBox.Show("Пожалуйста, выберите исполнителя.");
                return;
            }

            var newSubject = new Discipline
            {
                code_discipline = code_disciplineInt,
                volume = volumeInt,
                name = name,
                executor = selectedExecutor.surname
            };

            if (DataBaseManager.AddSubjects(newSubject))
            {
                DataBaseManager.UpdateDataBase();
                MessageBox.Show("Предмет успешно добавлен!");
                _subjectPage.LoadSubjectsList();
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении предмета.");
            }
        }
    }
}