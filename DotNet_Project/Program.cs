using System;
using System.Data.SqlClient;

class Program
{
    static string connStr = "Server=DESKTOP-VSR40M5\\SQLEXPRESS01;Database=CompanyDB_ADO;Integrated Security=True;";

    static void Main()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("========== Company Management System ==========");
            Console.WriteLine("1. Add Employee/Department");
            Console.WriteLine("2. Display Options");
            Console.WriteLine("3. Edit Employee/Department");
            Console.WriteLine("4. Delete Employee/Department");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1": AddMenu(); break;
                case "2": DisplayMenu(); break;
                case "3": EditMenu(); break;
                case "4": DeleteMenu(); break;
                case "5": running = false; break;
                default: Console.WriteLine("Invalid choice!"); break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void AddMenu()
    {
        Console.WriteLine("1. Add Employee\n2. Add Department");
        Console.Write("Choose: ");
        if (Console.ReadLine() == "1") AddEmployee();
        else AddDepartment();
    }

    static void DisplayMenu()
    {
        Console.WriteLine("1. All Employees\n2. All Departments\n3. Department with Employees");
        Console.Write("Choose: ");
        switch (Console.ReadLine())
        {
            case "1": DisplayEmployees(); break;
            case "2": DisplayDepartments(); break;
            case "3": DisplayDepartmentWithEmployees(); break;
            default: Console.WriteLine("Invalid choice!"); break;
        }
    }

    static void EditMenu()
    {
        Console.WriteLine("1. Edit Employee\n2. Edit Department");
        Console.Write("Choose: ");
        if (Console.ReadLine() == "1") EditEmployee();
        else EditDepartment();
    }

    static void DeleteMenu()
    {
        Console.WriteLine("1. Delete Employee\n2. Delete Department");
        Console.Write("Choose: ");
        if (Console.ReadLine() == "1") DeleteEmployee();
        else DeleteDepartment();
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
        Console.Write("Enter Department ID: ");
        int deptID = int.Parse(Console.ReadLine());

        string query = "INSERT INTO Employees (EmployeeName, DepartmentID) VALUES (@name, @deptID)";
        ExecuteQuery(query, cmd => {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@deptID", deptID);
        });
        Console.WriteLine("Employee added successfully.");
    }

    static void DisplayEmployees()
    {
        string query = "SELECT EmployeeID, EmployeeName FROM Employees";
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n========== Employees ==========");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["EmployeeID"]}, Name: {reader["EmployeeName"]}");
                }
            }
        }
    }

    static void DisplayDepartments()
    {
        string query = "SELECT * FROM Departments";
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n========== Departments ==========");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["DepartmentID"]}, Name: {reader["DepartmentName"]}");
                }
            }
        }
    }

    static void DisplayDepartmentWithEmployees()
    {
        Console.Write("Enter Department ID: ");
        int deptID = int.Parse(Console.ReadLine());
        string query = "SELECT EmployeeName FROM Employees WHERE DepartmentID = @deptID";
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@deptID", deptID);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n========== Employees in Department ==========");
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["EmployeeName"]);
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

        string query = "UPDATE Employees SET EmployeeName = @name WHERE EmployeeID = @id";
        ExecuteQuery(query, cmd => {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@id", id);
        });
        Console.WriteLine("Employee updated successfully.");
    }

    static void EditDepartment()
    {
        Console.Write("Enter Department ID to edit: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Enter New Department Name: ");
        string name = Console.ReadLine();

        string query = "UPDATE Departments SET DepartmentName = @name WHERE DepartmentID = @id";
        ExecuteQuery(query, cmd => {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@id", id);
        });
        Console.WriteLine("Department updated successfully.");
    }

    static void DeleteEmployee()
    {
        Console.Write("Enter Employee ID to delete: ");
        int id = int.Parse(Console.ReadLine());
        ExecuteQuery("DELETE FROM Employees WHERE EmployeeID = @id", cmd => cmd.Parameters.AddWithValue("@id", id));
        Console.WriteLine("Employee deleted.");
    }

    static void DeleteDepartment()
    {
        Console.Write("Enter Department ID to delete: ");
        int id = int.Parse(Console.ReadLine());
        ExecuteQuery("DELETE FROM Departments WHERE DepartmentID = @id", cmd => cmd.Parameters.AddWithValue("@id", id));
        Console.WriteLine("Department deleted.");
    }
}
