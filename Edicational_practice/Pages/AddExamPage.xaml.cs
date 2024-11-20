using Edicational_practice.Data;
using Edicational_practice.DbConnection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Edicational_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddExamPage.xaml
    /// </summary>
    public partial class AddExamPage : Page
    {
        private ExamPage _examPage;

        public AddExamPage(ExamPage examPage)
        {
            InitializeComponent();
            _examPage = examPage;
            LoadSubjects();
            LoadStudents();
            LoadEmployees();
            DatePicker.SelectedDate = DateTime.Today; 
            DatePicker.DisplayDateEnd = DateTime.Today; 
        }

        private void LoadSubjects()
        {
            var subjects = DataBaseManager.GetDisciplines();
            SubjectCB.ItemsSource = subjects;
            SubjectCB.DisplayMemberPath = "name";
        }

        private void LoadStudents()
        {
            var students = DataBaseManager.GetStudents();
            StudentCB.ItemsSource = students;
            StudentCB.DisplayMemberPath = "surname";
        }

        private void LoadEmployees()
        {
            var employees = DataBaseManager.GetEmployees();
            var teachers = employees.Where(e => e.post == "преподаватель").ToList();
            EmployeeCB.ItemsSource = teachers;
            EmployeeCB.DisplayMemberPath = "surname";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string idExam = IdExamTextBox.Text.Trim();
            DateTime? selectedDate = DatePicker.SelectedDate;
            string auditorium = AuditoriumTextBox.Text.Trim();
            string mark = MarkTextBox.Text.Trim();

            if (string.IsNullOrEmpty(idExam) || !selectedDate.HasValue || string.IsNullOrEmpty(auditorium) || string.IsNullOrEmpty(mark))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!int.TryParse(idExam, out int idExamInt))
            {
                MessageBox.Show("Идентификатор должен быть числом.");
                return;
            }

            if (!int.TryParse(mark, out int markInt))
            {
                MessageBox.Show("Оценка должна быть числом.");
                return;
            }

            var selectedSubject = (Discipline)SubjectCB.SelectedItem;
            var selectedStudent = (Student)StudentCB.SelectedItem;
            var selectedEmployee = (Employee)EmployeeCB.SelectedItem;

            if (selectedSubject == null || selectedStudent == null || selectedEmployee == null)
            {
                MessageBox.Show("Пожалуйста, выберите предмет, студента и преподавателя.");
                return;
            }

            var newExam = new Exam
            {
                id_exam = idExamInt,
                date_exam = selectedDate.Value,
                auditorium = auditorium,
                mark = markInt,
                Discipline = selectedSubject,
                Student = selectedStudent,
                Employee = selectedEmployee
            };

            if (DataBaseManager.AddExams(newExam))
            {
                DataBaseManager.UpdateDataBase();
                MessageBox.Show("Экзамен успешно добавлен!");
                _examPage.LoadExamsList();
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении экзамена.");
            }
        }
    }
}