using Isu.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Exceptions;

public class OGNPCourseException : Exception
{
    private OGNPCourseException(string? message)
       : base(message)
    {
    }

    public static OGNPCourseException InvalidCourse()
    {
        throw new OGNPCourseException("course is invalid");
    }

    public static OGNPCourseException InvalidCourseName()
    {
        throw new OGNPCourseException("courseName is invalid");
    }

    public static OGNPCourseException InvalidGroupName()
    {
        throw new OGNPCourseException("groupName is invalid");
    }

    public static OGNPCourseException InvalidMegaFaculty()
    {
        throw new OGNPCourseException("megaFaculty is invalid");
    }

    public static OGNPCourseException InvalidFlow(int flow)
    {
        throw new OGNPCourseException($"{flow} is invalid");
    }

    public static OGNPCourseException GroupIsFull(string? courseName)
    {
        throw new OGNPCourseException($"{courseName} is full");
    }

    public static OGNPCourseException StudentIsAlreadyExists(Student student)
    {
        throw new OGNPCourseException($"{student.FirstName + " " + student.FirstName} is already exists");
    }

    public static OGNPCourseException IntersectionWithExistingSchedule(Lesson lesson)
    {
        throw new OGNPCourseException($"{lesson.LessonName} intersects with the existing schedule");
    }
}
