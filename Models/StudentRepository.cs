namespace StudentMvcCrud.Models;

public class StudentRepository
{
    private readonly List<Student> students = new List<Student>();
    private int nextRollNo = 1;

    public List<Student> GetAll()
    {
        // Copies let the grid edit values before they are saved.
        List<Student> result = new List<Student>();
        foreach (Student student in students)
        {
            result.Add(new Student
            {
                RollNo = student.RollNo,
                Name = student.Name,
                AssignmentScore = student.AssignmentScore,
                ExamScore = student.ExamScore
            });
        }
        return result;
    }

    public Student? GetByRollNo(int rollNo)
    {
        foreach (Student student in students)
        {
            if (student.RollNo == rollNo)
                return student;
        }
        return null;
    }

    public void Add(Student student)
    {
        student.RollNo = nextRollNo;
        nextRollNo++;
        students.Add(student);
    }

    public void Update(Student student)
    {
        Student? savedStudent = GetByRollNo(student.RollNo);
        if (savedStudent != null)
        {
            savedStudent.Name = student.Name;
            savedStudent.AssignmentScore = student.AssignmentScore;
            savedStudent.ExamScore = student.ExamScore;
        }
    }

    public void Delete(int rollNo)
    {
        Student? student = GetByRollNo(rollNo);
        if (student != null)
            students.Remove(student);
    }
}
