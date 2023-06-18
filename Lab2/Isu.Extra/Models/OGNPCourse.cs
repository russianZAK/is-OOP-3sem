using Isu.Extra.Exceptions;
namespace Isu.Extra.Models;

public class OGNPCourse
{
    private readonly Dictionary<int, OGNPFlow> _flows;

    public OGNPCourse(string courseName, MegaFaculties megaFaculty)
    {
        if (courseName == null) OGNPCourseException.InvalidCourseName();

        FlowNumber = 0;
        _flows = new Dictionary<int, OGNPFlow>();
        CourseName = courseName!;
        MegaFaculty = megaFaculty;
    }

    public int FlowNumber { get; private set; }
    public string CourseName { get; }
    public MegaFaculties MegaFaculty { get; }

    public IReadOnlyCollection<OGNPFlow> Flows => _flows.Values.ToList();

    public void AddNewFlow(OGNPFlow newOGNPFlow)
    {
        if (newOGNPFlow == null) OGNPFlowException.InvalidOGNPFlow();

        _flows.Add(FlowNumber, newOGNPFlow!);
        FlowNumber++;
    }

    public void AddNewGroupToFlow(OGNPFlow flowOGNP, OGNPGroup newOGNPGroup)
    {
        if (flowOGNP == null) OGNPFlowException.InvalidOGNPFlow();
        if (newOGNPGroup == null) OGNPGroupException.InvalidGroup();
        if (Schedule.IsIntersectsWithExistingSchedule(newOGNPGroup!.Schedule.Lessons.ToList(), flowOGNP!.Schedule.Lessons.ToList())) OGNPFlowException.IntersectionWithExistingSchedule();

        _flows[flowOGNP!.Flow].AddNewOGNPGroup(newOGNPGroup!);
    }
}
