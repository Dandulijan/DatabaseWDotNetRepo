namespace DemoApp.Models
{
    public class Employee
    {
     public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime HDate { get; set; }
        public decimal Salary  { get; set; }
        public bool isActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
