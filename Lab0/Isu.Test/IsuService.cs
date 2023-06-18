using Isu.Entities;
using Isu.Exceptions;
using Isu.Services;
using Isu.Services.StudentsList;
using Xunit;

namespace Isu.Test;

public class IsuServiceTests
{
    private readonly IsuService service;

    public IsuServiceTests()
    {
        service = new IsuService();
    }

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        service.AddGroup("M32091");
        Group group = service.GetGroup("M32091");
        service.AddStudent(group, "Fisenko Nikita");
        Group groupWithStudent = service.GetGroup("M32091");
        Student studentWithGroup = service.GetStudent(0);
        Assert.Equal(studentWithGroup.Group, groupWithStudent);
        Assert.Equal(groupWithStudent.Students[0], studentWithGroup);
    }

    [Fact]
    public void ReachMaxStudentPerGroupThrowException()
    {
        service.AddStudent(service.AddGroup("M32091"), "Fisenko Nikita");
        Group group = service.GetGroup("M32091");

        var listOfStudents = new StudentsList();
        foreach (string student in listOfStudents.Students)
        {
            service.AddStudent(group, student);
        }

        Assert.Throws<GroupIsFullException>(() => service.AddStudent(group, "Trevor Mcneill"));
    }

    [Theory]
    [InlineData("CC3109")]
    [InlineData("fdgfgdf")]
    [InlineData("22343")]
    public void CreateGroupWithInvalidName_ThrowException(string invalidName)
    {
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(invalidName));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        service.AddStudent(service.AddGroup("M33091"), "Fisenko Nikita");
        service.AddGroup("M3109");
        Group oldGroup = service.GetGroup("M33091");

        service.ChangeStudentGroup(service.GetGroup("M33091").Students[0], service.GetGroup("M3109"));

        Group newGroup = service.GetGroup("M3109").Students[0].Group;

        Assert.NotEqual(oldGroup, newGroup);
        Assert.NotEqual(oldGroup.Students, newGroup.Students);
    }
}