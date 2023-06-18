using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraService : IsuService
{
    private readonly Dictionary<string, OGNPCourse> _coursesOGNP;
    private readonly Dictionary<string, Schedule> _groupsWithSchedule;
    private readonly Dictionary<int, List<OGNPGroup>> _studentsWithOGNPGroups;
    private readonly int maxOGNPGroupsPerStudent = 2;

    public IsuExtraService()
    {
        _groupsWithSchedule = new Dictionary<string, Schedule>();
        _coursesOGNP = new Dictionary<string, OGNPCourse>();
        _studentsWithOGNPGroups = new Dictionary<int, List<OGNPGroup>>();
        UpdateInformationOfGroups();
    }

    public IReadOnlyCollection<OGNPCourse> CoursesOGNP => _coursesOGNP.Values.ToList();

    public IReadOnlyDictionary<string, Schedule> GroupsWithSchedule => _groupsWithSchedule;

    public Lesson CreateNewLesson(DayOfWeek dayOfWeek, LessonNumber lessonNumber, string teacher, int auditorium, string lessonName)
    {
        CheckingNewLesson(teacher, auditorium, lessonName);

        var newLesson = new Lesson(dayOfWeek, lessonNumber, teacher!, auditorium, lessonName!);

        return newLesson;
    }

    public Schedule CreateNewSchedule()
    {
        return new Schedule();
    }

    public Schedule AddLessonToSchedule(Schedule schedule, Lesson lesson)
    {
        if (schedule == null) ScheduleException.InvalidSchedule();
        if (lesson == null) LessonException.InvalidLesson();

        schedule!.AddNewLesson(lesson!);

        return schedule;
    }

    public void AddScheduleToGroup(Schedule schedule, Group group)
    {
        UpdateInformationOfGroups();
        if (schedule == null) ScheduleException.InvalidSchedule();
        if (group == null) throw new InvalidGroupException(nameof(group));
        if (!_groupsWithSchedule.ContainsKey(group.GroupName)) throw new GroupDoesntExistsException(nameof(group));

        _groupsWithSchedule[group.GroupName] = schedule!;
    }

    public OGNPCourse CreateNewOGNPCourse(string courseName, MegaFaculties megaFaculty)
    {
        if (courseName == null) OGNPCourseException.InvalidCourseName();

        var newOGNPCourse = new OGNPCourse(courseName!, megaFaculty);
        if (_coursesOGNP.ContainsKey(courseName!)) ServiceException.OGNPCourseExistsInSystem(newOGNPCourse);

        _coursesOGNP.Add(courseName!, newOGNPCourse);

        return newOGNPCourse;
    }

    public OGNPFlow CreateNewFlowToOGNPCourse(Schedule schedule, string lecturer, OGNPCourse course, int auditorium)
    {
        if (lecturer == null) LessonException.InvalidTeacher(lecturer);
        if (auditorium < 0) LessonException.InvalidAuditorium(auditorium);
        if (course == null) OGNPCourseException.InvalidCourse();
        if (schedule == null) ScheduleException.InvalidSchedule();

        if (!_coursesOGNP.ContainsKey(course!.CourseName)) ServiceException.OGNPCourseDoesntExistInSystem(course);

        var newOGNPFlow = new OGNPFlow(schedule!, lecturer!, course, auditorium, course.FlowNumber);

        _coursesOGNP[course.CourseName].AddNewFlow(newOGNPFlow);

        return newOGNPFlow;
    }

    public OGNPGroup CreateNewOGNPGroup(Schedule schedule, OGNPFlow flow)
    {
        if (schedule == null) ScheduleException.InvalidSchedule();
        if (flow == null) OGNPFlowException.InvalidOGNPFlow();

        var newOGNPGroup = new OGNPGroup(schedule!, flow!, flow!.Groups.Count());

        if (Schedule.IsIntersectsWithExistingSchedule(schedule!.Lessons.ToList(), flow!.Schedule.Lessons.ToList())) ServiceException.IntersectionWithExistingSchedule();

        _coursesOGNP[flow!.OGNPCourse.CourseName].AddNewGroupToFlow(flow, newOGNPGroup);

        return newOGNPGroup;
    }

    public Lesson AddNewLessonToGroup(Lesson newLesson, Group group)
    {
        UpdateInformationOfGroups();

        if (newLesson == null) LessonException.InvalidLesson();
        if (group == null) throw new InvalidGroupException(nameof(group));
        if (!_groupsWithSchedule.ContainsKey(group.GroupName)) throw new GroupDoesntExistsException(group.GroupName);

        List<Lesson> groupLessons = GetGroupLessons(group);

        groupLessons.ForEach(lesson =>
        {
            if ((lesson.LessonTime.DayOfWeek == newLesson!.LessonTime.DayOfWeek) && (lesson.LessonTime.LessonNumber == newLesson.LessonTime.LessonNumber)) ServiceException.IntersectionWithExistingSchedule(newLesson);
        });

        _groupsWithSchedule[group.GroupName].AddNewLesson(newLesson!);

        return newLesson!;
    }

    public void EntryStudentToOGNPGroup(Student student, OGNPGroup group)
    {
        UpdateInformationOfGroups();

        if (group.OGNPFlow.OGNPCourse.MegaFaculty == MegaFaculties.TINT) ServiceException.SameFacultyOfGroupAndStudent();
        if (student == null) throw new InvalidStudentException(nameof(student));
        if (group == null) throw new InvalidGroupException(nameof(group));
        if (_studentsWithOGNPGroups[student.Id].Count() >= maxOGNPGroupsPerStudent) ServiceException.StudentHasEnoughOGNPCourses(student);

        Group groupOfSudent = GetGroup(student.Group.GroupName);

        if (_studentsWithOGNPGroups[student.Id].Count() == 1)
        {
            IEnumerable<Lesson> studentSchedule = group.OGNPFlow.Schedule.Lessons.ToList().Concat(group.Schedule.Lessons.ToList());
            IEnumerable<Lesson> newSchedule = _studentsWithOGNPGroups[student.Id].First().Schedule.Lessons.ToList().Concat(_studentsWithOGNPGroups[student.Id].First().OGNPFlow.Schedule.Lessons.ToList());

            if (Schedule.IsIntersectsWithExistingSchedule(studentSchedule.ToList(), newSchedule.ToList())) ServiceException.IntersectionWithExistingSchedule();
        }

        if (Schedule.IsIntersectsWithExistingSchedule(_groupsWithSchedule[groupOfSudent.GroupName].Lessons.ToList(), group.OGNPFlow.Schedule.Lessons.ToList())) ServiceException.IntersectionWithExistingSchedule();
        if (Schedule.IsIntersectsWithExistingSchedule(_groupsWithSchedule[groupOfSudent.GroupName].Lessons.ToList(), group.Schedule.Lessons.ToList())) ServiceException.IntersectionWithExistingSchedule();

        group.AddNewStudent(student);
        _studentsWithOGNPGroups[student.Id].Add(group);
    }

    public void RemoveStudentFromOGNPGroup(Student student, OGNPGroup group)
    {
        UpdateInformationOfGroups();

        if (student == null) throw new InvalidStudentException(nameof(student));
        if (group == null) OGNPGroupException.InvalidGroup();
        if (_studentsWithOGNPGroups[student.Id].Count() >= maxOGNPGroupsPerStudent) ServiceException.StudentHasEnoughOGNPCourses(student);

        if (!group!.Students.Contains(student)) OGNPGroupException.StudentDoesntExist(student);

        group.RemoveStudent(student);

        _studentsWithOGNPGroups[student.Id].Remove(group);
    }

    public List<OGNPFlow> GetOGNPFlows(OGNPCourse course)
    {
        if (course == null) OGNPCourseException.InvalidCourseName();

        return _coursesOGNP[course!.CourseName].Flows.ToList();
    }

    public List<Student> GetStudentListOfOGNPGroup(OGNPGroup group)
    {
        if (group == null) OGNPGroupException.InvalidGroup();

        return group!.Students.ToList();
    }

    public List<Student> StudentsWithoutOGNPCourses(Group group)
    {
        UpdateInformationOfGroups();
        if (group == null) throw new InvalidGroupException(nameof(group));
        if (!_groupsWithSchedule.ContainsKey(group.GroupName)) throw new GroupDoesntExistsException(nameof(group));

        var students = new List<Student>();

        group.Students.ForEach(student =>
        {
            if (_studentsWithOGNPGroups[student.Id].Count < 2) students.Add(student);
        });

        return students;
    }

    private void UpdateInformationOfGroups()
    {
        var allGroups = Groups.ToList();

        allGroups.ForEach(group =>
        {
            if (!_groupsWithSchedule.ContainsKey(group.GroupName))
            {
                _groupsWithSchedule.Add(group.GroupName, new Schedule());

                group.Students.ForEach(student =>
                {
                    if (!_studentsWithOGNPGroups.ContainsKey(student.Id)) _studentsWithOGNPGroups.Add(student.Id, new List<OGNPGroup>());
                });
            }
        });
    }

    private void CheckingNewLesson(string teacher, int auditorium, string lessonName)
    {
        if (teacher == null) LessonException.InvalidTeacher(teacher);
        if (auditorium < 0) LessonException.InvalidAuditorium(auditorium);
        if (lessonName == null) LessonException.InvalidLessonName(lessonName);
    }

    private List<Lesson> GetGroupLessons(Group group)
    {
        if (group == null) throw new InvalidGroupException(nameof(group));
        if (!_groupsWithSchedule.ContainsKey(group.GroupName)) throw new GroupDoesntExistsException(group.GroupName);

        return _groupsWithSchedule[group.GroupName].Lessons.ToList();
    }
}
