using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Lesson
{
    public Lesson(DayOfWeek dayOfWeek, LessonNumber lessonNumber, string teacher, int auditorium, string lessonName)
    {
        if (teacher == null) LessonException.InvalidTeacher(teacher);
        if (auditorium < 0) LessonException.InvalidAuditorium(auditorium);
        if (lessonName == null) LessonException.InvalidLessonName(lessonName);

        LessonTime = new LessonTime(dayOfWeek, lessonNumber);

        LessonName = lessonName!;

        Teacher = teacher!;

        Auditorium = auditorium;
    }

    public LessonTime LessonTime { get; }
    public string LessonName { get; }
    public string Teacher { get; }
    public int Auditorium { get; }
}
