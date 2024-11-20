using Edicational_practice.DbConnection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Edicational_practice.Data
{
    internal static class DataBaseManager
    {
        private static Edicational_practice_321Entities1 _dbConnectoin = new Edicational_practice_321Entities1();

        public static bool UpdateDataBase()
        {
            try
            {
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<Discipline> GetDisciplines()
        {
            return _dbConnectoin.Discipline.ToList();
        }

        public static List<Teacher> GetTeachers()
        {
            return _dbConnectoin.Teacher.ToList();
        }

        public static List<Exam> GetExams()
        {
            return _dbConnectoin.Exam.ToList();
        }

        public static List<Student> GetStudents()
        {
            return _dbConnectoin.Student.ToList();
        }

        public static List<Employee> GetEmployees()
        {
            return _dbConnectoin.Employee.ToList();
        }

        public static List<Department> GetDepartments()
        {
            return _dbConnectoin.Department.ToList();
        }

        public static List<Faculty> GetFaculties()
        {
            return _dbConnectoin.Faculty.ToList();
        }

        public static Employee GetEmployee(int employeeId)
        {
            return _dbConnectoin.Employee.FirstOrDefault(e => e.id_employee == employeeId);
        }

        public static bool AddSubjects(Discipline d)
        {
            try
            {
                _dbConnectoin.Discipline.Add(d);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddTeachers(Employee t)
        {
            try
            {
                _dbConnectoin.Employee.Add(t);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddExams(Exam e)
        {
            try
            {
                _dbConnectoin.Exam.Add(e);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddDepartments(Department d)
        {
            try
            {
                _dbConnectoin.Department.Add(d);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveTeachers(Employee te)
        {
            try
            {
                _dbConnectoin.Employee.Remove(te);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveSubjects(Discipline di)
        {
            try
            {
                _dbConnectoin.Discipline.Remove(di);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveExams(Exam ex)
        {
            try
            {
                _dbConnectoin.Exam.Remove(ex);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveDepartments(Department de)
        {
            try
            {
                _dbConnectoin.Department.Remove(de);
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetDepartmentCodeByEmployeeId(int employeeId)
        {
            switch (employeeId)
            {
                case 101: return "пи";
                case 201: return "ис";
                case 301: return "мм";
                case 401: return "оф";
                case 501: return "вм";
                case 601: return "эф";
                case 0: return "эф";
                default: return null;
            }
        }

        public static bool UpdateDepartment(Department department)
        {
            try
            {
                if (string.IsNullOrEmpty(department.cipher))
                {
                    MessageBox.Show("Некорректный идентификатор кафедры.");
                    return false;
                }

                _dbConnectoin.Department.Attach(department);
                _dbConnectoin.Entry(department).State = EntityState.Modified;
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении кафедры: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateTeacher(Employee employee)
        {
            try
            {
                if (employee.id_employee == 0)
                {
                    MessageBox.Show("Некорректный идентификатор учителя.");
                    return false;
                }

                _dbConnectoin.Employee.Attach(employee);
                _dbConnectoin.Entry(employee).State = EntityState.Modified;
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении учителя: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateDiscipline(Discipline discipline)
        {
            try
            {
                if (discipline.code_discipline == 0)
                {
                    MessageBox.Show("Некорректный идентификатор предмета.");
                    return false;
                }

                _dbConnectoin.Discipline.Attach(discipline);
                _dbConnectoin.Entry(discipline).State = EntityState.Modified;
                _dbConnectoin.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении предмета: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateExam(Exam exam, string userRole)
        {
            try
            {
                if (exam.id_exam == 0)
                {
                    MessageBox.Show("Некорректный идентификатор экзамена.");
                    return false;
                }

                var existingExam = _dbConnectoin.Exam.Find(exam.id_exam);
                if (existingExam != null)
                {
                    if (userRole == "преподаватель")
                    {
                        existingExam.mark = exam.mark;
                    }
                    else
                    {
                        existingExam.date_exam = exam.date_exam;
                        existingExam.auditorium = exam.auditorium;
                        existingExam.mark = exam.mark;
                    }

                    _dbConnectoin.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении экзамена: {ex.Message}");
                return false;
            }
        }
    }
}