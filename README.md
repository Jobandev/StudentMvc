# Student Manager

Open `StudentMvcCrud.csproj` in Visual Studio and press F5. The project is also included in `WinFormsMvcCrud.sln`; right-click **StudentMvcCrud** and choose **Set as Startup Project** to run it from that solution.

## Files

- `Models/Student.cs`: the four student properties.
- `Models/StudentRepository.cs`: a private readonly list and basic add, read, update, and delete methods.
- `Views/StudentView.cs`: form events, input validation, and displaying data.
- `Views/StudentView.Designer.cs`: the controls and layout.
- `Controllers/StudentController.cs`: connects form events to repository methods.
- `Program.cs`: creates three sample students and starts the form.

This version uses named event handlers, `if` statements, and `foreach` loops. The controller uses the form directly to keep this beginner version small.

## Using the form

- Enter Name, Assignment Score, and Exam Score, then click **Add**. Roll No is assigned automatically: 1, 2, 3, and so on.
- Double-click Name or a score in the grid, edit it, and press Enter to save. Press Esc to cancel.
- Enter a Roll No and click **Find** (or press Enter) to locate a student. You can then edit the top fields and click **Update**.
- Select a student and click **Delete selected**. A confirmation appears before deletion.
- **Clear** resets the fields and selection.
- Use Ctrl or Shift to select several rows. Bulk checkboxes then appear. Enter replacement values in the top fields, check the fields to change, and click **Update selected**. Only checked fields change. **Delete selected** also works for multiple students.

Scores use `decimal` and must be non-negative. No maximum score is assumed. Success messages appear on the form; invalid input shows an error.

Data is kept in memory. Every launch starts a fresh list, resets the roll-number counter, and adds the three samples. Deleted roll numbers are not reused during that run.
