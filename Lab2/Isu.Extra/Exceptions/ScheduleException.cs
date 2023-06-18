using Isu.Extra.Models;

namespace Isu.Extra.Exceptions;

public class ScheduleException : Exception
{
    private ScheduleException(string? message)
       : base(message)
    {
    }

    public static ScheduleException IntersectionWithExistingSchedule(Lesson newLesson)
    {
        throw new ScheduleException($"{newLesson.LessonName} intersects with the existing schedule");
    }

    public static ScheduleException LessonIsNotInSchedule(Lesson lesson)
    {
        throw new ScheduleException($"{lesson.LessonName} is not in the schedule");
    }

    public static ScheduleException InvalidSchedule()
    {
        throw new ScheduleException("Schedule is invalid");
    }
}
