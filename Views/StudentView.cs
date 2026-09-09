using StudentMvcCrud.Models;

namespace StudentMvcCrud.Views;

public partial class StudentView : Form
{
    public event EventHandler? LoadStudents;
    public event EventHandler? AddStudent;
    public event EventHandler? UpdateStudent;
    public event EventHandler? DeleteStudents;
    public event EventHandler? FindStudent;
    public event EventHandler? UpdateSelectedStudents;
    public event EventHandler? SaveGridStudent;

    public Student? EditedStudent { get; private set; }
    public string SearchRollNo { get { return txtSearch.Text; } }
    public bool ChangeName { get { return chkName.Checked; } }
    public bool ChangeAssignment { get { return chkAssignment.Checked; } }
    public bool ChangeExam { get { return chkExam.Checked; } }

    public StudentView()
    {
        InitializeComponent();
        Load += FormLoaded;
        btnAdd.Click += AddClicked;
        btnUpdate.Click += UpdateClicked;
        btnDelete.Click += DeleteClicked;
        btnFind.Click += FindClicked;
        btnClear.Click += ClearClicked;
        btnBulkUpdate.Click += BulkUpdateClicked;
        txtSearch.KeyDown += SearchKeyDown;
        studentsGrid.SelectionChanged += SelectionChanged;
        studentsGrid.CellValidating += ValidateCell;
        studentsGrid.CellEndEdit += CellEdited;
        studentsGrid.DataError += GridError;
        studentsGrid.CellDoubleClick += CellDoubleClicked;
    }

    private void FormLoaded(object? sender, EventArgs e) { LoadStudents?.Invoke(this, EventArgs.Empty); }
    private void AddClicked(object? sender, EventArgs e) { AddStudent?.Invoke(this, EventArgs.Empty); }
    private void UpdateClicked(object? sender, EventArgs e) { UpdateStudent?.Invoke(this, EventArgs.Empty); }
    private void DeleteClicked(object? sender, EventArgs e) { DeleteStudents?.Invoke(this, EventArgs.Empty); }
    private void FindClicked(object? sender, EventArgs e) { FindStudent?.Invoke(this, EventArgs.Empty); }
    private void BulkUpdateClicked(object? sender, EventArgs e) { UpdateSelectedStudents?.Invoke(this, EventArgs.Empty); }
    private void ClearClicked(object? sender, EventArgs e) { ClearFields(); }

    private void SearchKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            FindStudent?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<int> GetSelectedRollNos()
    {
        List<int> rollNos = new List<int>();
        foreach (DataGridViewRow row in studentsGrid.SelectedRows)
        {
            if (row.DataBoundItem is Student student)
                rollNos.Add(student.RollNo);
        }
        return rollNos;
    }

    public void ShowStudents(List<Student> students)
    {
        studentsGrid.DataSource = students;
        studentsGrid.Columns["RollNo"].HeaderText = "Roll No";
        studentsGrid.Columns["RollNo"].ReadOnly = true;
        studentsGrid.Columns["AssignmentScore"].HeaderText = "Assignment Score";
        studentsGrid.Columns["ExamScore"].HeaderText = "Exam Score";
        ClearFields();
    }

    public void ClearFields()
    {
        studentsGrid.ClearSelection();
        studentsGrid.CurrentCell = null;
        txtRollNo.Clear();
        txtName.Clear();
        txtAssignment.Text = "0";
        txtExam.Text = "0";
        txtSearch.Clear();
        chkName.Checked = false;
        chkAssignment.Checked = false;
        chkExam.Checked = false;
    }

    private void ShowStudent(Student student)
    {
        txtRollNo.Text = student.RollNo.ToString();
        txtName.Text = student.Name;
        txtAssignment.Text = student.AssignmentScore.ToString();
        txtExam.Text = student.ExamScore.ToString();
    }

    private void SelectionChanged(object? sender, EventArgs e)
    {
        bulkPanel.Visible = studentsGrid.SelectedRows.Count > 1;
        if (studentsGrid.SelectedRows.Count == 1)
        {
            Student student = (Student)studentsGrid.SelectedRows[0].DataBoundItem;
            ShowStudent(student);
        }
        else
        {
            txtRollNo.Clear();
        }
    }

    public void SelectRollNo(int rollNo)
    {
        studentsGrid.ClearSelection();
        foreach (DataGridViewRow row in studentsGrid.Rows)
        {
            Student student = (Student)row.DataBoundItem;
            if (student.RollNo == rollNo)
            {
                studentsGrid.CurrentCell = row.Cells[0];
                row.Selected = true;
                studentsGrid.FirstDisplayedScrollingRowIndex = row.Index;
                ShowStudent(student);
                return;
            }
        }
    }

    public Student? ReadFields(bool bulk)
    {
        Student student = new Student();
        if (!bulk || ChangeName)
        {
            if (txtName.Text.Trim() == "")
            {
                ShowError("Enter a student name.");
                return null;
            }
            student.Name = txtName.Text.Trim();
        }
        if (!bulk || ChangeAssignment)
        {
            decimal score;
            if (!decimal.TryParse(txtAssignment.Text, out score) || score < 0)
            {
                ShowError("Assignment Score must be a non-negative number.");
                return null;
            }
            student.AssignmentScore = score;
        }
        if (!bulk || ChangeExam)
        {
            decimal score;
            if (!decimal.TryParse(txtExam.Text, out score) || score < 0)
            {
                ShowError("Exam Score must be a non-negative number.");
                return null;
            }
            student.ExamScore = score;
        }
        return student;
    }

    private void CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !studentsGrid.Columns[e.ColumnIndex].ReadOnly)
            studentsGrid.BeginEdit(true);
    }

    private void ValidateCell(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (!studentsGrid.IsCurrentCellInEditMode) return;
        string field = studentsGrid.Columns[e.ColumnIndex].DataPropertyName;
        string value = Convert.ToString(e.FormattedValue) ?? "";
        string error = "";
        decimal score;
        if (field == "Name" && value.Trim() == "")
            error = "Name cannot be empty.";
        if ((field == "AssignmentScore" || field == "ExamScore") &&
            (!decimal.TryParse(value, out score) || score < 0))
            error = "Score must be a non-negative number.";
        e.Cancel = error != "";
        studentsGrid.Rows[e.RowIndex].ErrorText = error;
        if (e.Cancel) lblStatus.Text = error + " Press Esc to cancel.";
    }

    private void CellEdited(object? sender, DataGridViewCellEventArgs e)
    {
        studentsGrid.Rows[e.RowIndex].ErrorText = "";
        EditedStudent = (Student)studentsGrid.Rows[e.RowIndex].DataBoundItem;
        SaveGridStudent?.Invoke(this, EventArgs.Empty);
        ShowStudent(EditedStudent);
    }

    private void GridError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        e.Cancel = true;
        lblStatus.Text = "Enter a valid value. Press Esc to cancel.";
    }

    public void ShowMessage(string message) { lblStatus.Text = message; }
    public void ShowError(string message)
    {
        MessageBox.Show(this, message, "Student error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    public bool Confirm(string message)
    {
        return MessageBox.Show(this, message, "Confirm", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
    }
}
