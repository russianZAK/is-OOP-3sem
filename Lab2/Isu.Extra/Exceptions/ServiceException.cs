using Isu.Entities;
using Isu.Extra.Models;
namespace Isu.Extra.Exceptions;

public class ServiceException : Exception
{
    private ServiceException(string? message)
       : base(message)
    {
    }

    public static ServiceException IntersectionWithExistingSchedule(Lesson lesson)
    {
        throw new ServiceException($"{lesson.LessonName} intersects with the existing schedule");
    }

    public static ServiceException IntersectionWithExistingSchedule()
    {
        throw new ServiceException("lessons intersect with the existing schedule");
    }

    public static ServiceException OGNPCourseDoesntExistInSystem(OGNPCourse courseOGNP)
    {
        throw new ServiceException($"{courseOGNP.CourseName} doesn't exist in system");
    }

    public static ServiceException StudentHasEnoughOGNPCourses(Student student)
    {
        throw new ServiceException($"{student.FirstName + " " + student.LastName} has enough OGNP courses");
    }

    public static ServiceException OGNPCourseExistsInSystem(OGNPCourse courseOGNP)
    {
        throw new ServiceException($"{courseOGNP.CourseName} already exists in system");
    }

    public static ServiceException SameFacultyOfGroupAndStudent()
    {
        throw new ServiceException("student from the same faculty");
    }
}
