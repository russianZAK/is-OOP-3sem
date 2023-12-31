using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService
{
    Group AddGroup(string groupName);
    Student AddStudent(Group group, string name);

    Student GetStudent(int id);
    Student? FindStudent(int id);
    List<Student> FindStudents(string groupName);
    List<Student> FindStudents(CourseNumber courseNumber);

    Group? FindGroup(string groupName);
    List<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}