using System;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


class Program
{
    static string connStr =
"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CompanyDB_ADO;Integrated Security=True;Trust Server Certificate=False;";

    static void Main()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("========== Employee Management System ==========");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Add Department");
            Console.WriteLine("3. Display Employees");
            Console.WriteLine("4. Display Departments");
            Console.WriteLine("5. Search Employee");
            Console.WriteLine("6. Edit Employee");
            Console.WriteLine("7. Delete Employee");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1": AddEmployee(); break;
                case "2": AddDepartment(); break;
                case "3": DisplayEmployees(); break;
                case "4": DisplayDepartments(); break;
                case "5": SearchEmployee(); break;
                case "6": EditEmployee(); break;
                case "7": DeleteEmployee(); break;
                case "8": running = false; break;
                default: Console.WriteLine("Invalid choice!"); break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void ExecuteQuery(string query, Action<SqlCommand> parameterSetter = null)
    {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                parameterSetter?.Invoke(cmd);
                cmd.ExecuteNonQuery();
            }
        }
    }

    static void AddDepartment()
    {
        Console.Write("Enter Department Name: ");
        string deptName = Console.ReadLine();

        string query = "INSERT INTO Departments (DepartmentName) VALUES (@deptName)";
        ExecuteQuery(query, cmd => cmd.Parameters.AddWithValue("@deptName", deptName));

        Console.WriteLine("Department added successfully.");
    }

    static void AddEmployee()
    {
        Console.Write("Enter Employee Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Salary: ");
        decimal salary = decimal.Parse(Console.ReadLine());

        Console.Write("Enter Age: ");
        int age = int.Parse(Console.ReadLine());

        Console.Write("Enter Gender (Male/Female): ");
        string gender = Console.ReadLine();

        Console.Write("Enter Department ID: ");
        int deptID = int.Parse(Console.ReadLine());

        string query = "INSERT INTO Employees (EmployeeName, Salary, Age, Gender, DepartmentID) VALUES (@name, @salary, @age, @gender, @deptID)";
        ExecuteQuery(query, cmd =>
        {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@salary", salary);
            cmd.Parameters.AddWithValue("@age", age);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@deptID", deptID);
        });

        Console.WriteLine("Employee added successfully.");
    }

    static void DisplayEmployees()
    {
        string query = "SELECT EmployeeID, EmployeeName, Salary, Age, Gender, DepartmentName " +
                       "FROM Employees INNER JOIN Departments ON Employees.DepartmentID = Departments.DepartmentID";

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n========== Employee List ==========");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["EmployeeID"]}, Name: {reader["EmployeeName"]}, Salary: {reader["Salary"]}, " +
                                      $"Age: {reader["Age"]}, Gender: {reader["Gender"]}, Department: {reader["DepartmentName"]}");
                }
            }
        }
    }

    static void DisplayDepartments()
    {
        string query = "SELECT d.DepartmentName, e.EmployeeName " +
                       "FROM Departments d LEFT JOIN Employees e ON d.DepartmentID = e.DepartmentID";

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n========== Department List ==========");
                string lastDept = "";
                while (reader.Read())
                {
                    string dept = reader["DepartmentName"].ToString();
                    if (dept != lastDept)
                    {
                        Console.WriteLine($"Department: {dept}");
                        lastDept = dept;
                    }

                    if (reader["EmployeeName"] != DBNull.Value)
                    {
                        Console.WriteLine($"   - {reader["EmployeeName"]}");
                    }
                }
            }
        }
    }

    static void SearchEmployee()
    {
        Console.Write("Enter Employee Name or ID: ");
        string input = Console.ReadLine();

        string query = "SELECT * FROM Employees WHERE EmployeeID = @input OR EmployeeName = @input";

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@input", input);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["EmployeeID"]}, Name: {reader["EmployeeName"]}, Salary: {reader["Salary"]}, " +
                                          $"Age: {reader["Age"]}, Gender: {reader["Gender"]}");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }
                }
            }
        }
    }

    static void EditEmployee()
    {
        Console.Write("Enter Employee ID to edit: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter New Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter New Salary: ");
        decimal salary = decimal.Parse(Console.ReadLine());

        Console.Write("Enter New Age: ");
        int age = int.Parse(Console.ReadLine());

        Console.Write("Enter New Gender (Male/Female): ");
        string gender = Console.ReadLine();

        Console.Write("Enter New Department ID: ");
        int deptID = int.Parse(Console.ReadLine());

        string query = "UPDATE Employees SET EmployeeName = @name, Salary = @salary, Age = @age, Gender = @gender, DepartmentID = @deptID WHERE EmployeeID = @id";
        ExecuteQuery(query, cmd =>
        {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@salary", salary);
            cmd.Parameters.AddWithValue("@age", age);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@deptID", deptID);
            cmd.Parameters.AddWithValue("@id", id);
        });

        Console.WriteLine("Employee updated successfully.");
    }

    static void DeleteEmployee()
    {
        Console.Write("Enter Employee ID to delete: ");
        int id = int.Parse(Console.ReadLine());

        string query = "DELETE FROM Employees WHERE EmployeeID = @id";
        ExecuteQuery(query, cmd => cmd.Parameters.AddWithValue("@id", id));

        Console.WriteLine("Employee deleted successfully.");
    }
}
