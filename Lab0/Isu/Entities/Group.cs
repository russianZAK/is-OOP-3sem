using Isu.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private readonly int lenghtOfGroupNameFirstCourse = 5;

    private readonly int lenghtOfGroupNameOtherCourses = 6;

    private readonly char studentIsBachelor = '3';

    private readonly char studentIsMagistracy = '4';

    public Group(string groupName)
    {
        if (groupName == null) throw new InvalidGroupNameException(nameof(groupName));

        Parsing(groupName);

        GroupName = groupName;

        Students = new List<Student>();
    }

    public string GroupName { get; }

    public MegaFaculties MegaFaculty { get; private set; }

    public CourseNumber CourseNumber { get; private set; }

    public List<Student> Students { get; }

    private void Parsing(string groupName)
    {
        char typeOfEducation = groupName[1];
        char firstLetter = groupName[0];

        if (!char.IsLetter(firstLetter)) throw new InvalidGroupNameException(groupName);
        if (typeOfEducation != studentIsBachelor && typeOfEducation != studentIsMagistracy) throw new InvalidGroupNameException(groupName);
        if (!int.TryParse(groupName[2].ToString(), out int courseNumber))
        {
            throw new InvalidGroupNameException(groupName);
        }

        ParsingMegaFaculty(firstLetter, groupName);

        string numbersAfterLetter = groupName.Substring(1);
        if (!int.TryParse(numbersAfterLetter, out int _)) throw new InvalidGroupNameException(groupName);

        switch (courseNumber)
        {
            case 1:

                IsGroupNameFits(groupName.Length, lenghtOfGroupNameFirstCourse, groupName);
                CourseNumber = CourseNumber.First;
                break;

            case 2:

                IsGroupNameFits(groupName.Length, lenghtOfGroupNameOtherCourses, groupName);
                CourseNumber = CourseNumber.Second;
                break;

            case 3:

                if (!IsValidCourseForMagistracy(studentIsMagistracy, typeOfEducation))
                {
                        throw new InvalidGroupNameException(groupName);
                }

                IsGroupNameFits(groupName.Length, lenghtOfGroupNameOtherCourses, groupName);
                CourseNumber = CourseNumber.Third;
                break;

            case 4:

                if (!IsValidCourseForMagistracy(studentIsMagistracy, typeOfEducation))
                {
                    throw new InvalidGroupNameException(groupName);
                }

                IsGroupNameFits(groupName.Length, lenghtOfGroupNameOtherCourses, groupName);
                CourseNumber = CourseNumber.Fourth;
                break;

            default:
                throw new InvalidGroupNameException(groupName);
        }
    }

    private void ParsingMegaFaculty(char firstLetter, string groupName)
    {
        switch (firstLetter)
        {
            case 'A':
                MegaFaculty = MegaFaculties.CTaM;
                break;

            case 'B':
                MegaFaculty = MegaFaculties.BLTS;
                break;

            case 'C':
                MegaFaculty = MegaFaculties.TMI;
                break;

            case 'D':
                MegaFaculty = MegaFaculties.IDP;
                break;

            case 'E':
                MegaFaculty = MegaFaculties.PT;
                break;

            case 'F':
                MegaFaculty = MegaFaculties.CTaM;
                break;

            case 'H':
                MegaFaculty = MegaFaculties.PT;
                break;

            case 'J':
                MegaFaculty = MegaFaculties.IDP;
                break;

            case 'K':
                MegaFaculty = MegaFaculties.CTaM;
                break;

            case 'L':
                MegaFaculty = MegaFaculties.PT;
                break;

            case 'M':
                MegaFaculty = MegaFaculties.TINT;
                break;

            case 'N':
                MegaFaculty = MegaFaculties.CTaM;
                break;

            case 'O':
                MegaFaculty = MegaFaculties.BLTS;
                break;

            case 'P':
                MegaFaculty = MegaFaculties.CTaM;
                break;

            case 'R':
                MegaFaculty = MegaFaculties.PT;
                break;

            case 'S':
                MegaFaculty = MegaFaculties.TINT;
                break;

            case 'T':
                MegaFaculty = MegaFaculties.BLTS;
                break;

            case 'U':
                MegaFaculty = MegaFaculties.TMI;
                break;

            case 'V':
                MegaFaculty = MegaFaculties.PT;
                break;

            case 'W':
                MegaFaculty = MegaFaculties.BLTS;
                break;

            case 'Z':
                MegaFaculty = MegaFaculties.PT;
                break;

            default:
                throw new InvalidGroupNameException(groupName);
        }
    }

    private void IsGroupNameFits(int lenght, int fittedLenght, string groupName)
    {
        if (lenght != fittedLenght)
        {
            throw new InvalidGroupNameException(groupName);
        }
    }

    private bool IsValidCourseForMagistracy(char studentIsMagistracy, char typeOfEducation)
    {
        if (typeOfEducation == studentIsMagistracy)
        {
            return false;
        }

        return true;
    }
}