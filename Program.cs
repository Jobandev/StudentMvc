using StudentMvcCrud.Models;
using StudentMvcCrud.Views;
using StudentMvcCrud.Controllers;

namespace StudentMvcCrud;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        StudentRepository repository = new StudentRepository();
        repository.Add(new Student { Name = "Ali", AssignmentScore = 75, ExamScore = 80 });
        repository.Add(new Student { Name = "Sara", AssignmentScore = 85, ExamScore = 90 });
        repository.Add(new Student { Name = "John", AssignmentScore = 65, ExamScore = 70 });

        StudentView view = new StudentView();
        StudentController controller = new StudentController(view, repository);
        Application.Run(view);
    }
}
