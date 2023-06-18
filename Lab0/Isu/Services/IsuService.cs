using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly Dictionary<string, Group> _groups;
    private readonly int capacityOfGroups = 30;
    private int ids = 0;

    public IsuService()
    {
        _groups = new Dictionary<string, Group>();
    }

    public IReadOnlyCollection<Group> Groups
    {
        get
        {
            return _groups.Values.ToList();
        }
    }

    public Group AddGroup(string groupName)
    {
        if (groupName == null) throw new InvalidGroupNameException(nameof(groupName));

        if (_groups.ContainsKey(groupName)) throw new GroupIsAlreadyExistsException(groupName);

        var group = new Group(groupName);

        _groups.Add(groupName, group);

        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        if (group == null) throw new InvalidGroupException(nameof(group));

        if (name == null) throw new InvalidStudentNameException(nameof(name));

        if (!_groups.ContainsKey(group.GroupName))
        {
            throw new GroupDoesntExistsException(group.GroupName);
        }

        if (_groups[group.GroupName].Students.Count >= capacityOfGroups)
        {
            throw new GroupIsFullException(_groups[group.GroupName].GroupName);
        }

        var newStudent = new Student(name, group);
        newStudent.Id = ids;
        ids++;
        _groups[group.GroupName].Students.Add(newStudent);

        return newStudent;
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (student == null) throw new InvalidStudentException(nameof(student));

        if (newGroup == null) throw new InvalidGroupException(nameof(newGroup));

        if (!_groups.ContainsKey(newGroup.GroupName))
        {
            throw new GroupDoesntExistsException(newGroup.GroupName);
        }

        if (student.Group.GroupName == newGroup.GroupName)
        {
            throw new StudentHasGroupAndGroupContainsStudentException(student.LastName);
        }

        if (_groups[newGroup.GroupName].Students.Count >= capacityOfGroups)
        {
            throw new GroupIsFullException(_groups[newGroup.GroupName].GroupName);
        }

        _groups[student.Group.GroupName].Students.Remove(student);

        _groups[newGroup.GroupName].Students.Add(student);
        student.ChangeGroup(newGroup);
    }

    public Group? FindGroup(string groupName)
    {
        if (groupName == null) throw new InvalidGroupNameException(nameof(groupName));

        if (!_groups.ContainsKey(groupName))
        {
            return null;
        }

        return _groups[groupName];
    }

    public Group GetGroup(string groupName)
    {
        if (groupName == null) throw new InvalidGroupNameException(nameof(groupName));

        if (!_groups.ContainsKey(groupName))
        {
            throw new GroupDoesntExistsException(groupName);
        }

        return _groups[groupName];
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        var findingGroups = new List<Group>();

        foreach (Group findingGroup in _groups.Values)
        {
            if (findingGroup.CourseNumber == courseNumber)
            {
                findingGroups.Add(findingGroup);
            }
        }

        return findingGroups;
    }

    public Student? FindStudent(int id)
    {
        foreach (Group findingGroup in _groups.Values)
        {
            foreach (Student findingStudent in findingGroup.Students)
            {
                if (findingStudent.Id == id)
                {
                    return findingStudent;
                }
            }
        }

        return null;
    }

    public List<Student> FindStudents(string groupName)
    {
        if (groupName == null) throw new InvalidGroupNameException(nameof(groupName));

        var findingStudents = new List<Student>();
        if (_groups.ContainsKey(groupName))
        {
            findingStudents = _groups[groupName].Students;
        }

        return findingStudents;
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        var findingStudents = new List<Student>();

        foreach (Group findingGroup in _groups.Values)
        {
            if (findingGroup.CourseNumber == courseNumber)
            {
                findingStudents.AddRange(findingGroup.Students);
            }
        }

        return findingStudents;
    }

    public Student GetStudent(int id)
    {
        foreach (Group findingGroup in _groups.Values)
        {
            foreach (Student findingStudent in findingGroup.Students)
            {
                if (findingStudent.Id == id)
                {
                    return findingStudent;
                }
            }
        }

        throw new InvalidStudentIdException(id);
    }
}