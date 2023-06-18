namespace Isu.Extra.Exceptions;

public class LessonTimeException : Exception
{
    private LessonTimeException(string? message)
       : base(message)
    {
    }

    public static LessonTimeException InvalidLessonTimeDayOfWeek()
    {
        throw new LessonTimeException("day of week is invalid");
    }

    public static LessonTimeException InvalidLessonTimeNumber(int lessonNumber)
    {
        throw new LessonTimeException($"{lessonNumber} is invalid");
    }
}