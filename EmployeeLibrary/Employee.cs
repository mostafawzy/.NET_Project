using System;
using System.Collections.Generic;

namespace EmployeeLibrary
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Human
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public Human(string name, int age, Gender gender)
        {
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}, Gender: {Gender}";
        }
    }

    public class Employee : Human
    {
        private static int idCounter = 1;
        public int ID { get; private set; }
        public double Salary { get; set; }

        public Employee(string name, double salary, int age, Gender gender)
            : base(name, age, gender)
        {
            ID = idCounter++;
            Salary = salary;
        }

        public override string ToString()
        {
            return $"ID: {ID}, {base.ToString()}, Salary: {Salary:C}";
        }

        public void DisplayData()
        {
            Console.WriteLine(this.ToString());
            Console.WriteLine("----------------------------");
        }
    }

    // Sorting by Name
    public class SortByName : IComparer<Employee>
    {
        public int Compare(Employee e1, Employee e2)
        {
            return e1.Name.CompareTo(e2.Name);
        }
    }

    // Sorting by Salary (Descending)
    public class SortBySalary : IComparer<Employee>
    {
        public int Compare(Employee e1, Employee e2)
        {
            return e2.Salary.CompareTo(e1.Salary);
        }
    }

    // Sorting by ID
    public class SortByID : IComparer<Employee>
    {
        public int Compare(Employee e1, Employee e2)
        {
            return e1.ID.CompareTo(e2.ID);
        }
    }
}
