using Isu.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTest
{
    private readonly IsuExtraService service;

    public IsuExtraServiceTest()
    {
        service = new IsuExtraService();
    }

    [Fact]
    public void AddNewOGNPCourse()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;

        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Assert.Equal(cyberSecurityOGNPCourse, service.CoursesOGNP.First());
    }

    [Fact]
    public void EntryStudentToOGNPCourse()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;

        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule scheduleForOGNPFlow = service.CreateNewSchedule();
        Lesson lessonForOGNPFlow1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        Lesson lessonForOGNPFlow2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        scheduleForOGNPFlow = service.AddLessonToSchedule(scheduleForOGNPFlow, lessonForOGNPFlow1);
        scheduleForOGNPFlow = service.AddLessonToSchedule(scheduleForOGNPFlow, lessonForOGNPFlow2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(scheduleForOGNPFlow, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroup = service.CreateNewSchedule();
        Lesson lessonForGroup1 = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.First, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroup2 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup1);
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroup, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");
        Student student = service.AddStudent(group, "Nikita Fisenko");

        service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup);
        List<Student> studentList = service.GetStudentListOfOGNPGroup(newOGNPGroupOfCyberSecurityGroup);

        var students = new List<Student>();
        students.Add(student);

        Assert.Equal(students, studentList);
    }

    [Fact]
    public void EntryStudentToOGNPCourse_IntersectionWithExistingStudentSchedule()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule scheduleForOGNPFlow = service.CreateNewSchedule();
        Lesson lessonForOGNPFlow1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson lessonForOGNPFlow2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        scheduleForOGNPFlow = service.AddLessonToSchedule(scheduleForOGNPFlow, lessonForOGNPFlow1);
        scheduleForOGNPFlow = service.AddLessonToSchedule(scheduleForOGNPFlow, lessonForOGNPFlow2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(scheduleForOGNPFlow, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroup = service.CreateNewSchedule();
        Lesson lessonForGroup1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Third, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroup2 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup1);
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroup, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");
        Student student = service.AddStudent(group, "Nikita Fisenko");

        Schedule scheduleForStudentGroup = service.CreateNewSchedule();
        Lesson lessonForStudentGroup1 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher1", 165, "Programming");
        Lesson lessonForStudentGroup2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher1", 165, "Programming");

        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup1);
        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup2);

        service.AddScheduleToGroup(scheduleForStudentGroup, group);

        Assert.Throws<ServiceException>(() => service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup));
    }

    [Fact]
    public void EntryStudentToOGNPCourse_IntersectionWithExistingOldOGNPGroupSchedule()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;

        string courseName2 = "maths";
        MegaFaculties megaFaculty2 = MegaFaculties.PT;

        OGNPCourse mathsOGNPCourse = service.CreateNewOGNPCourse(courseName2, megaFaculty2);
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule scheduleForOGNPFlow = service.CreateNewSchedule();
        Lesson lessonFlow = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Maths");
        Lesson lessonFlow2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Third, "Teacher", 122, "Maths");
        scheduleForOGNPFlow = service.AddLessonToSchedule(scheduleForOGNPFlow, lessonFlow);
        scheduleForOGNPFlow = service.AddLessonToSchedule(scheduleForOGNPFlow, lessonFlow2);

        OGNPFlow newOGNPFlowOfMaths = service.CreateNewFlowToOGNPCourse(scheduleForOGNPFlow, "Teacher3", mathsOGNPCourse, 543);

        Schedule scheduleForMathGroup = service.CreateNewSchedule();
        Lesson lessonForMathGroup1 = service.CreateNewLesson(DayOfWeek.Monday, LessonNumber.Second, "Teacher", 522, "Maths");
        Lesson lessonForMathGroup2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 344, "Maths");
        scheduleForMathGroup = service.AddLessonToSchedule(scheduleForMathGroup, lessonForMathGroup1);
        scheduleForMathGroup = service.AddLessonToSchedule(scheduleForMathGroup, lessonForMathGroup2);

        OGNPGroup newOGNPGroupOfMathGroup = service.CreateNewOGNPGroup(scheduleForMathGroup, newOGNPFlowOfMaths);

        Schedule scheduleOfCyberSecurity = service.CreateNewSchedule();
        Lesson lessonOfCyberSecurity1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Seventh, "Teacher", 122, "Cyber Security");
        Lesson lessonOfCyberSecurity2 = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.Fiveth, "Teacher", 122, "Cyber Security");
        scheduleOfCyberSecurity = service.AddLessonToSchedule(scheduleOfCyberSecurity, lessonOfCyberSecurity1);
        scheduleOfCyberSecurity = service.AddLessonToSchedule(scheduleOfCyberSecurity, lessonOfCyberSecurity2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(scheduleOfCyberSecurity, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroupOfCyberSecurity = service.CreateNewSchedule();
        Lesson lessonForGroupOfCyberSecurity1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Sixth, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroupOfCyberSecurity2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroupOfCyberSecurity = service.AddLessonToSchedule(scheduleForGroupOfCyberSecurity, lessonForGroupOfCyberSecurity1);
        scheduleForGroupOfCyberSecurity = service.AddLessonToSchedule(scheduleForGroupOfCyberSecurity, lessonForGroupOfCyberSecurity2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroupOfCyberSecurity, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");
        Student student = service.AddStudent(group, "Nikita Fisenko");

        Schedule scheduleForStudentGroup = service.CreateNewSchedule();
        Lesson lessonForStudentGroup1 = service.CreateNewLesson(DayOfWeek.Monday, LessonNumber.First, "Teacher1", 165, "Programming");
        Lesson lessonForStudentGroup2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Fiveth, "Teacher1", 165, "Programming");

        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup1);
        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup2);

        service.AddScheduleToGroup(scheduleForStudentGroup, group);

        service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup);

        Assert.Throws<ServiceException>(() => service.EntryStudentToOGNPGroup(student, newOGNPGroupOfMathGroup));
    }

    [Fact]
    public void EntryStudentToOGNPCourse_IntersectionWithExistingFlowSchedule()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule scheduleOfCyberSecurity = service.CreateNewSchedule();
        Lesson lessonOfCyberSecurity1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson lessonOfCyberSecurity2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        scheduleOfCyberSecurity = service.AddLessonToSchedule(scheduleOfCyberSecurity, lessonOfCyberSecurity1);
        scheduleOfCyberSecurity = service.AddLessonToSchedule(scheduleOfCyberSecurity, lessonOfCyberSecurity2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(scheduleOfCyberSecurity, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroupOfCyberSecurity = service.CreateNewSchedule();
        Lesson lessonForGroupOfCyberSecurity1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Third, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroupOfCyberSecurity2 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroupOfCyberSecurity = service.AddLessonToSchedule(scheduleForGroupOfCyberSecurity, lessonForGroupOfCyberSecurity1);
        scheduleForGroupOfCyberSecurity = service.AddLessonToSchedule(scheduleForGroupOfCyberSecurity, lessonForGroupOfCyberSecurity2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroupOfCyberSecurity, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");
        Student student = service.AddStudent(group, "Nikita Fisenko");

        Schedule scheduleForStudentGroup = service.CreateNewSchedule();
        Lesson lessonForStudentGroup1 = service.CreateNewLesson(DayOfWeek.Monday, LessonNumber.First, "Teacher1", 165, "Programming");
        Lesson lessonForStudentGroup2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher1", 165, "Programming");

        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup1);
        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup2);

        service.AddScheduleToGroup(scheduleForStudentGroup, group);

        Assert.Throws<ServiceException>(() => service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup));
    }

    [Fact]
    public void EntryStudentToOGNPCourse_SameFaculty()
    {
        string courseName = "Programming";
        MegaFaculties megaFaculty = MegaFaculties.TINT;
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule scheduleOfProgramming = service.CreateNewSchedule();
        Lesson lessonOfProgramming1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson lessonOfProgramming2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        scheduleOfProgramming = service.AddLessonToSchedule(scheduleOfProgramming, lessonOfProgramming1);
        scheduleOfProgramming = service.AddLessonToSchedule(scheduleOfProgramming, lessonOfProgramming2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(scheduleOfProgramming, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroup = service.CreateNewSchedule();
        Lesson lessonForGroup1 = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.First, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroup2 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup1);
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroup, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");
        Student student = service.AddStudent(group, "Nikita Fisenko");

        Assert.Throws<ServiceException>(() => service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup));
    }

    [Fact]
    public void RemoveStudentFromOGNPGroup()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule scheduleOfCyberSecurity = service.CreateNewSchedule();
        Lesson lessonOfCyberSecurity1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson lessonOfCyberSecurity2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        scheduleOfCyberSecurity = service.AddLessonToSchedule(scheduleOfCyberSecurity, lessonOfCyberSecurity1);
        scheduleOfCyberSecurity = service.AddLessonToSchedule(scheduleOfCyberSecurity, lessonOfCyberSecurity2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(scheduleOfCyberSecurity, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroup = service.CreateNewSchedule();
        Lesson lessonForGroup1 = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.First, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroup2 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup1);
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroup, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");
        Student student = service.AddStudent(group, "Nikita Fisenko");

        service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup);
        service.RemoveStudentFromOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup);

        List<Student> studentList = service.GetStudentListOfOGNPGroup(newOGNPGroupOfCyberSecurityGroup);

        var students = new List<Student>();
        students.Add(student);

        Assert.NotEqual(students, studentList);
    }

    [Fact]
    public void GetFlowsOfOGNPCourse()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule schedule = service.CreateNewSchedule();
        Lesson lesson1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson lesson2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        schedule = service.AddLessonToSchedule(schedule, lesson1);
        schedule = service.AddLessonToSchedule(schedule, lesson2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(schedule, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForSecondOGNPFlow = service.CreateNewSchedule();
        Lesson lessonForSecondOGNPFlow = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson secondLessonForSecondOGNPFlow = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        scheduleForSecondOGNPFlow = service.AddLessonToSchedule(scheduleForSecondOGNPFlow, lessonForSecondOGNPFlow);
        scheduleForSecondOGNPFlow = service.AddLessonToSchedule(scheduleForSecondOGNPFlow, secondLessonForSecondOGNPFlow);

        OGNPFlow secondOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(scheduleForSecondOGNPFlow, "Teacher", cyberSecurityOGNPCourse, 122);

        var listOfLows = new List<OGNPFlow>();
        listOfLows.Add(newOGNPFlowOfCyberSecurity);
        listOfLows.Add(secondOGNPFlowOfCyberSecurity);

        Assert.Equal(service.GetOGNPFlows(cyberSecurityOGNPCourse), listOfLows);
    }

    [Fact]
    public void GetStudentListOfOGNPGroup()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule schedule = service.CreateNewSchedule();
        Lesson lesson1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson lesson2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        schedule = service.AddLessonToSchedule(schedule, lesson1);
        schedule = service.AddLessonToSchedule(schedule, lesson2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(schedule, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroup = service.CreateNewSchedule();
        Lesson lessonForGroup1 = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.First, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroup2 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup1);
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroup, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");
        Group group2 = service.AddGroup("M33091");
        Group group3 = service.AddGroup("M34091");

        Student student = service.AddStudent(group, "Nikita Fisenko");
        Student student2 = service.AddStudent(group2, "Alex Kovalev");
        Student student3 = service.AddStudent(group3, "Max Akimov");
        Student student4 = service.AddStudent(group, "Andru Stars");

        Schedule scheduleForStudentGroup = service.CreateNewSchedule();
        Lesson lessonForStudentGroup1 = service.CreateNewLesson(DayOfWeek.Monday, LessonNumber.First, "Teacher1", 165, "Programming");
        Lesson lessonForStudentGroup2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Fourth, "Teacher1", 165, "Programming");

        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup1);
        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup2);

        service.AddScheduleToGroup(scheduleForStudentGroup, group);

        service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup);
        service.EntryStudentToOGNPGroup(student2, newOGNPGroupOfCyberSecurityGroup);
        service.EntryStudentToOGNPGroup(student3, newOGNPGroupOfCyberSecurityGroup);
        service.EntryStudentToOGNPGroup(student4, newOGNPGroupOfCyberSecurityGroup);
        List<Student> studentList = service.GetStudentListOfOGNPGroup(newOGNPGroupOfCyberSecurityGroup);

        var students = new List<Student>();
        students.Add(student);
        students.Add(student2);
        students.Add(student3);
        students.Add(student4);

        Assert.Equal(students, studentList);
    }

    [Fact]
    public void StudentsWithoutOGNPCourses()
    {
        string courseName = "cybersecurity";
        MegaFaculties megaFaculty = MegaFaculties.CTaM;
        OGNPCourse cyberSecurityOGNPCourse = service.CreateNewOGNPCourse(courseName, megaFaculty);

        Schedule schedule = service.CreateNewSchedule();
        Lesson lesson1 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.First, "Teacher", 122, "Cyber Security");
        Lesson lesson2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Second, "Teacher", 122, "Cyber Security");
        schedule = service.AddLessonToSchedule(schedule, lesson1);
        schedule = service.AddLessonToSchedule(schedule, lesson2);

        OGNPFlow newOGNPFlowOfCyberSecurity = service.CreateNewFlowToOGNPCourse(schedule, "Teacher", cyberSecurityOGNPCourse, 122);

        Schedule scheduleForGroup = service.CreateNewSchedule();
        Lesson lessonForGroup1 = service.CreateNewLesson(DayOfWeek.Friday, LessonNumber.First, "Teacher", 522, "Cyber Security");
        Lesson lessonForGroup2 = service.CreateNewLesson(DayOfWeek.Thursday, LessonNumber.Third, "Teacher", 344, "Cyber Security");
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup1);
        scheduleForGroup = service.AddLessonToSchedule(scheduleForGroup, lessonForGroup2);

        OGNPGroup newOGNPGroupOfCyberSecurityGroup = service.CreateNewOGNPGroup(scheduleForGroup, newOGNPFlowOfCyberSecurity);

        Group group = service.AddGroup("M32091");

        Student student = service.AddStudent(group, "Nikita Fisenko");
        Student student2 = service.AddStudent(group, "Andru Stars");
        Student student3 = service.AddStudent(group, "Max Martin");

        Schedule scheduleForStudentGroup = service.CreateNewSchedule();
        Lesson lessonForStudentGroup1 = service.CreateNewLesson(DayOfWeek.Monday, LessonNumber.First, "Teacher1", 165, "Programming");
        Lesson lessonForStudentGroup2 = service.CreateNewLesson(DayOfWeek.Tuesday, LessonNumber.Fourth, "Teacher1", 165, "Programming");

        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup1);
        scheduleForStudentGroup = service.AddLessonToSchedule(scheduleForStudentGroup, lessonForStudentGroup2);

        service.AddScheduleToGroup(scheduleForStudentGroup, group);

        service.EntryStudentToOGNPGroup(student, newOGNPGroupOfCyberSecurityGroup);
        service.EntryStudentToOGNPGroup(student2, newOGNPGroupOfCyberSecurityGroup);
        List<Student> studentList = service.StudentsWithoutOGNPCourses(group);

        var studentsWithoutOGNPCourses = new List<Student>();
        studentsWithoutOGNPCourses.Add(student);
        studentsWithoutOGNPCourses.Add(student2);
        studentsWithoutOGNPCourses.Add(student3);

        Assert.Equal(studentsWithoutOGNPCourses, studentList);
    }
}
