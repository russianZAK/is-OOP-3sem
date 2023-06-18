namespace Isu.Extra.Models;

public class LessonTime
{
    public LessonTime(DayOfWeek dayOfWeek, LessonNumber lessonNumber)
    {
        DayOfWeek = dayOfWeek;
        LessonNumber = lessonNumber;
    }

    public DayOfWeek DayOfWeek { get; }
    public LessonNumber LessonNumber { get; }
}
