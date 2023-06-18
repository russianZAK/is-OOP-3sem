namespace Isu.Extra.Exceptions;

public class LessonException : Exception
{
    private LessonException(string? message)
       : base(message)
    {
    }

    public static LessonException InvalidLesson()
    {
        throw new LessonException("lesson is invalid");
    }

    public static LessonException InvalidDayOfWeek(string? dayOfWeek)
    {
        throw new LessonException($"{dayOfWeek} is invalid");
    }

    public static LessonException InvalidLessonName(string? lessonName)
    {
        throw new LessonException($"{lessonName} is invalid");
    }

    public static LessonException InvalidLessonNumber(int lessonNumber)
    {
        throw new LessonException($"{lessonNumber} is invalid");
    }

    public static LessonException InvalidTeacher(string? teacher)
    {
        throw new LessonException($"{teacher} is invalid");
    }

    public static LessonException InvalidAuditorium(int auditorium)
    {
        throw new LessonException($"{auditorium} is invalid");
    }
}
