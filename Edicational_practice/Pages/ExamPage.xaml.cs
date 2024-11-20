using Edicational_practice.Data;
using Edicational_practice.DbConnection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для ExamPage.xaml
    /// </summary>
    public partial class ExamPage : Page
    {
        private string userRole;
        public bool IsReadOnly { get; set; }
        private List<Exam> originalExams;

        public ExamPage(string role)
        {
            InitializeComponent();
            userRole = role;
            LoadExamsList();
            UpdateUI();
            LoadSubjects();
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
                    IsReadOnly = false;
                    break;
                default:
                    AddBt.Visibility = Visibility.Collapsed;
                    DelBt.Visibility = Visibility.Collapsed;
                    SaveBt.Visibility = Visibility.Collapsed;
                    IsReadOnly = false;
                    break;
            }
            ExamDataGrid.IsReadOnly = IsReadOnly;

            // Разрешить редактирование только столбца с оценками для преподавателя
            if (userRole == "преподаватель")
            {
                foreach (var column in ExamDataGrid.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        if (textColumn.Binding is Binding binding && binding.Path.Path == "mark")
                        {
                            textColumn.IsReadOnly = false;
                        }
                        else
                        {
                            textColumn.IsReadOnly = true;
                        }
                    }
                }
            }
        }

        public void LoadExamsList()
        {
            originalExams = DataBaseManager.GetExams();
            ExamDataGrid.ItemsSource = originalExams;
        }

        private void LoadSubjects()
        {
            var subjects = originalExams.Select(e => e.Discipline).Distinct().ToList();
            SubjectFilterCB.Items.Clear(); // Очистка ComboBox перед установкой нового ItemsSource
            SubjectFilterCB.ItemsSource = subjects;
        }

        private void AddExamButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddExamPage(this));
        }

        private void DeleteExamButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamDataGrid.SelectedItem is Exam selectedExam)
            {
                if (DataBaseManager.RemoveExams(selectedExam))
                {
                    MessageBox.Show("Экзамен успешно удален!");
                    LoadExamsList();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении экзамена.");
                }
            }
            else
            {
                MessageBox.Show("Выберите экзамен для удаления.");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterExams();
        }

        private void SubjectFilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterExams();
        }

        private void MarkFilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterExams();
        }

        private void FilterExams()
        {
            var filteredExams = originalExams.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                filteredExams = filteredExams.Where(e => e.Student.surname.Contains(SearchTextBox.Text));
            }

            if (SubjectFilterCB.SelectedItem is Discipline selectedSubject && SubjectFilterCB.SelectedItem != null && !(SubjectFilterCB.SelectedItem is ComboBoxItem))
            {
                filteredExams = filteredExams.Where(e => e.Discipline.name == selectedSubject.name);
            }

            if (MarkFilterCB.SelectedItem is ComboBoxItem selectedMarkItem && selectedMarkItem.Content is string selectedMark && selectedMark != "All")
            {
                filteredExams = filteredExams.Where(e => e.mark.ToString() == selectedMark);
            }

            ApplySorting(filteredExams);
        }

        private void ApplySorting(IQueryable<Exam> exams)
        {
            if (SortColumnCB.SelectedItem is ComboBoxItem selectedColumnItem && selectedColumnItem.Content is string selectedColumn)
            {
                var sortDirection = SortDirectionCB.SelectedItem is ComboBoxItem selectedDirectionItem && selectedDirectionItem.Content is string selectedDirection && selectedDirection == "Descending"
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;

                switch (selectedColumn)
                {
                    case "ID":
                        exams = sortDirection == ListSortDirection.Ascending ? exams.OrderBy(e => e.id_exam) : exams.OrderByDescending(e => e.id_exam);
                        break;
                    case "Дата":
                        exams = sortDirection == ListSortDirection.Ascending ? exams.OrderBy(e => e.date_exam) : exams.OrderByDescending(e => e.date_exam);
                        break;
                    case "Дисциплина":
                        exams = sortDirection == ListSortDirection.Ascending ? exams.OrderBy(e => e.Discipline.name) : exams.OrderByDescending(e => e.Discipline.name);
                        break;
                    case "Студент":
                        exams = sortDirection == ListSortDirection.Ascending ? exams.OrderBy(e => e.Student.surname) : exams.OrderByDescending(e => e.Student.surname);
                        break;
                    case "Преподаватель":
                        exams = sortDirection == ListSortDirection.Ascending ? exams.OrderBy(e => e.Employee.surname) : exams.OrderByDescending(e => e.Employee.surname);
                        break;
                    case "Аудитория":
                        exams = sortDirection == ListSortDirection.Ascending ? exams.OrderBy(e => e.auditorium) : exams.OrderByDescending(e => e.auditorium);
                        break;
                    case "Оценка":
                        exams = sortDirection == ListSortDirection.Ascending ? exams.OrderBy(e => e.mark) : exams.OrderByDescending(e => e.mark);
                        break;
                }
            }

            ExamDataGrid.ItemsSource = exams.ToList();
        }

        private void SortColumnCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterExams();
        }

        private void SortDirectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterExams();
        }

        private void ExamDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedExam = e.Row.Item as Exam;
                if (editedExam != null)
                {
                    if (!DataBaseManager.UpdateExam(editedExam, userRole))
                    {
                        MessageBox.Show("Ошибка при обновлении данных экзамена.");
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;

            foreach (var exam in ExamDataGrid.ItemsSource.OfType<Exam>())
            {
                if (!DataBaseManager.UpdateExam(exam, userRole))
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                MessageBox.Show("Данные экзаменов успешно сохранены!");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных экзаменов.");
            }
        }
    }
}