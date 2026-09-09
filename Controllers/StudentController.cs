using StudentMvcCrud.Models;
using StudentMvcCrud.Views;

namespace StudentMvcCrud.Controllers;

public class StudentController
{
    private readonly StudentView view;
    private readonly StudentRepository repository;

    public StudentController(StudentView view, StudentRepository repository)
    {
        this.view = view;
        this.repository = repository;
        view.LoadStudents += Load;
        view.AddStudent += Add;
        view.UpdateStudent += Update;
        view.DeleteStudents += Delete;
        view.FindStudent += Find;
        view.UpdateSelectedStudents += UpdateSelected;
        view.SaveGridStudent += SaveGrid;
    }

    private void Load(object? sender, EventArgs e)
    {
        view.ShowStudents(repository.GetAll());
    }

    private void Add(object? sender, EventArgs e)
    {
        Student? student = view.ReadFields(false);
        if (student == null) return;
        repository.Add(student);
        view.ShowStudents(repository.GetAll());
        view.ShowMessage("Student added. Roll No: " + student.RollNo);
    }

    private void Update(object? sender, EventArgs e)
    {
        List<int> rollNos = view.GetSelectedRollNos();
        if (rollNos.Count != 1)
        {
            view.ShowError("Find or select one student first.");
            return;
        }
        Student? student = view.ReadFields(false);
        if (student == null) return;
        student.RollNo = rollNos[0];
        repository.Update(student);
        view.ShowStudents(repository.GetAll());
        view.ShowMessage("Student updated.");
    }

    private void Delete(object? sender, EventArgs e)
    {
        List<int> rollNos = view.GetSelectedRollNos();
        if (rollNos.Count == 0)
        {
            view.ShowError("Find or select a student first.");
            return;
        }
        if (!view.Confirm("Delete " + rollNos.Count + " selected student(s)?")) return;
        foreach (int rollNo in rollNos)
            repository.Delete(rollNo);
        view.ShowStudents(repository.GetAll());
        view.ShowMessage("Selected students deleted.");
    }

    private void Find(object? sender, EventArgs e)
    {
        string search = view.SearchRollNo;
        view.ClearFields();
        int rollNo;
        if (!int.TryParse(search, out rollNo) || rollNo <= 0)
        {
            view.ShowError("Enter a positive whole number for Roll No.");
            return;
        }
        if (repository.GetByRollNo(rollNo) == null)
        {
            view.ShowError("No student found with Roll No " + rollNo);
            return;
        }
        view.SelectRollNo(rollNo);
        view.ShowMessage("Student found. Edit a grid cell or use the fields above.");
    }

    private void UpdateSelected(object? sender, EventArgs e)
    {
        List<int> rollNos = view.GetSelectedRollNos();
        if (rollNos.Count == 0)
        {
            view.ShowError("Select students first.");
            return;
        }
        if (!view.ChangeName && !view.ChangeAssignment && !view.ChangeExam)
        {
            view.ShowError("Choose which fields to update.");
            return;
        }
        Student? values = view.ReadFields(true);
        if (values == null) return;
        if (!view.Confirm("Apply the checked fields to " + rollNos.Count + " students?")) return;
        foreach (int rollNo in rollNos)
        {
            Student? student = repository.GetByRollNo(rollNo);
            if (student != null)
            {
                if (view.ChangeName) student.Name = values.Name;
                if (view.ChangeAssignment) student.AssignmentScore = values.AssignmentScore;
                if (view.ChangeExam) student.ExamScore = values.ExamScore;
            }
        }
        view.ShowStudents(repository.GetAll());
        view.ShowMessage("Selected students updated.");
    }

    private void SaveGrid(object? sender, EventArgs e)
    {
        if (view.EditedStudent != null)
        {
            Student? saved = repository.GetByRollNo(view.EditedStudent.RollNo);
            if (saved == null) return;
            if (saved.Name == view.EditedStudent.Name &&
                saved.AssignmentScore == view.EditedStudent.AssignmentScore &&
                saved.ExamScore == view.EditedStudent.ExamScore) return;
            repository.Update(view.EditedStudent);
            view.ShowMessage("Saved student " + view.EditedStudent.RollNo);
        }
    }
}
