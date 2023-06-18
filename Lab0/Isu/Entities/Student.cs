using Isu.Models;

namespace Isu.Entities;

public class Student
{
    private int _id;

    public Student(string name, Group group)
    {
        string[] firstAndLastNames = name.Split();
        FirstName = firstAndLastNames[0];
        LastName = firstAndLastNames[1];
        Group = group;
        CourseNumber = group.CourseNumber;
    }

    public string FirstName { get; }

    public string LastName { get; }

    public Group Group { get; private set; }

    public CourseNumber CourseNumber { get; private set; }

    public int Id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    public void ChangeGroup(Group group)
    {
        Group = group;
        CourseNumber = group.CourseNumber;
    }
}