namespace StudentMvcCrud.Models;

public class Student
{
    public int RollNo { get; set; }
    public string Name { get; set; } = "";
    public decimal AssignmentScore { get; set; }
    public decimal ExamScore { get; set; }
}
