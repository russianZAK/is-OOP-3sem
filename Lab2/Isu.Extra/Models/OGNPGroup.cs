using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class OGNPGroup
{
    private readonly Dictionary<int, Student> _students;
    private readonly int capacityOfGroup = 30;

    public OGNPGroup(Schedule schedule, OGNPFlow flow, int numberOfGroup)
    {
        if (schedule == null) ScheduleException.InvalidSchedule();
        if (flow == null) OGNPFlowException.InvalidOGNPFlow();
        Lecturer = flow!.Lecturer;
        _students = new Dictionary<int, Student>();
        Schedule = schedule!;
        OGNPFlow = flow!;
        GroupName = flow!.OGNPCourse.CourseName + flow!.Flow + "." + numberOfGroup;
    }

    public OGNPFlow OGNPFlow { get; }
    public Schedule Schedule { get; }
    public string Lecturer { get; }
    public string GroupName { get; }

    public IReadOnlyCollection<Student> Students => _students.Values.ToList();

    public void AddNewStudent(Student newStudent)
    {
        if (OGNPFlow.OGNPCourse.MegaFaculty == newStudent.Group.MegaFaculty) ServiceException.SameFacultyOfGroupAndStudent();
        if (newStudent == null) throw new InvalidStudentException(nameof(newStudent));
        if (_students.ContainsKey(newStudent.Id)) OGNPGroupException.StudentIsAlreadyExists(newStudent);
        if (_students.Count >= capacityOfGroup) OGNPGroupException.GroupIsFull();

        _students.Add(newStudent.Id, newStudent);
    }

    public void RemoveStudent(Student student)
    {
        if (student == null) throw new InvalidStudentException(nameof(student));

        _students.Remove(student.Id);
    }
}
