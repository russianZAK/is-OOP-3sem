using Isu.Extra.Exceptions;
namespace Isu.Extra.Models;

public class OGNPFlow
{
    private readonly List<OGNPGroup> _groups;

    public OGNPFlow(Schedule schedule, string lecturer, OGNPCourse course, int auditorium, int flow)
    {
        if (lecturer == null) LessonException.InvalidTeacher(lecturer);
        if (auditorium < 0) LessonException.InvalidAuditorium(auditorium);
        if (course == null) OGNPCourseException.InvalidCourse();
        if (schedule == null) ScheduleException.InvalidSchedule();
        if (flow < 0) OGNPCourseException.InvalidFlow(flow);

        _groups = new List<OGNPGroup>();
        Schedule = schedule!;
        Lecturer = lecturer!;
        OGNPCourse = course!;
        Auditorium = auditorium;
        Flow = flow;
    }

    public IReadOnlyCollection<OGNPGroup> Groups => _groups;

    public int Flow { get; }
    public Schedule Schedule { get; }
    public string Lecturer { get; }
    public OGNPCourse OGNPCourse { get; }
    public int Auditorium { get; }

    public void AddNewOGNPGroup(OGNPGroup newOGNPGroup)
    {
        newOGNPGroup = newOGNPGroup ?? throw new ArgumentNullException(nameof(newOGNPGroup));

        if (_groups.Contains(newOGNPGroup!)) OGNPFlowException.GroupIsAlreadyExists();
        if (Schedule.IsIntersectsWithExistingSchedule(Schedule.Lessons.ToList(), newOGNPGroup!.Schedule.Lessons.ToList())) OGNPFlowException.IntersectionWithExistingSchedule();

        _groups.Add(newOGNPGroup!);
    }

    public void RemoveOGNPGroup(OGNPGroup newOGNPGroup)
    {
        newOGNPGroup = newOGNPGroup ?? throw new ArgumentNullException(nameof(newOGNPGroup));
        if (!_groups.Contains(newOGNPGroup!)) OGNPFlowException.GroupDoesntExist();

        _groups.Remove(newOGNPGroup!);
    }
}
