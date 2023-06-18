using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Schedule
{
    private readonly Dictionary<LessonTime, Lesson> _schedule;

    public Schedule()
    {
        _schedule = new Dictionary<LessonTime, Lesson>();
    }

    public IReadOnlyCollection<Lesson> Lessons => _schedule.Values.ToList();

    public static bool IsIntersectsWithExistingSchedule(List<Lesson> checkingIntersectionWithSchedule, List<Lesson> schedule)
    {
        foreach (Lesson lesson in checkingIntersectionWithSchedule)
        {
            if (schedule.Any(scheduleLesson => scheduleLesson.LessonTime.DayOfWeek == lesson.LessonTime.DayOfWeek && scheduleLesson.LessonTime.LessonNumber == lesson.LessonTime.LessonNumber)) return true;
        }

        return false;
    }

    public void AddNewLesson(Lesson newLesson)
    {
        if (newLesson == null) LessonException.InvalidLesson();

        if (_schedule.ContainsKey(newLesson!.LessonTime)) ScheduleException.IntersectionWithExistingSchedule(newLesson);

        _schedule.Add(newLesson.LessonTime, newLesson);
    }

    public void RemoveLesson(Lesson removingLesson)
    {
        if (removingLesson == null) LessonException.InvalidLesson();

        if (!_schedule.ContainsKey(removingLesson!.LessonTime)) ScheduleException.LessonIsNotInSchedule(removingLesson);

        _schedule.Remove(removingLesson.LessonTime);
    }
}
