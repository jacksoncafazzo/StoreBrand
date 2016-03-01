using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HardKnockRegistrar
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CoursesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Course.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Course firstCourse = new Course("Life of the Past: Oregon", "GEO117");
      Course secondCourse = new Course("Life of the Past: Oregon", "GEO117");

      //Assert
      Assert.Equal(firstCourse, secondCourse);
    }

    [Fact]
    public void Test_Save_SavesCourseToDatabase()
    {
       //Arrange
       Course testCourse = new Course("Linguistical Problem Solving Programs", "LING324");
       testCourse.Save();

       //Act
       List<Course> result = Course.GetAll();
       List<Course> testList = new List<Course>{testCourse};

       //Assert
       Assert.Equal(testList, result);
    }
//
    [Fact]
    public void Test_Save_AssignsIdToCourseObject()
    {
      //Arrange
      Course testCourse = new Course("Enterin' SQL Commands", "CS145");
      testCourse.Save();

      //Act
      Course savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCourseInDatabase()
    {
      //Arrange
      Course testCourse = new Course("Screwin' Up Da'bases With Chris", "SUD399");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, foundCourse);
    }

//     [Fact]
//     public void Test_GetStudents_RetrievesAllStudentsWithCourse()
//     {
//       Course testCourse = new Course("Household chores");
//       testCourse.Save();
//
//       Student firstStudent = new Student("Mow the lawn", testCourse.GetId());
//       firstStudent.Save();
//       Student secondStudent = new Student("Do the dishes", testCourse.GetId());
//       secondStudent.Save();
//
//       List<Student> testStudentList = new List<Student> {firstStudent, secondStudent};
//       List<Student> resultStudentList = testCourse.GetStudents();
//
//       Assert.Equal(testStudentList, resultStudentList);
//     }
//
//     [Fact]
//     public void Test_Delete_DeletesCourseFromDatabase()
//     {
//       //Arrange
//       string name1 = "Home stuff";
//       Course testCourse1 = new Course(name1);
//       testCourse1.Save();
//
//       string name2 = "Work stuff";
//       Course testCourse2 = new Course(name2);
//       testCourse2.Save();
//
//       //Act
//       testCourse1.Delete();
//       List<Course> resultCourses = Course.GetAll();
//       List<Course> testCourseList = new List<Course> {testCourse2};
//
//       //Assert
//       Assert.Equal(testCourseList, resultCourses);
//     }
//
//     [Fact]
//     public void Test_AddStudent_AddsStudentToCourse()
//     {
//       //Arrange
//       Course testCourse = new Course("Household chores");
//       testCourse.Save();
//
//       Student testStudent = new Student("Mow the lawn");
//       testStudent.Save();
//
//       Student testStudent2 = new Student("Water the garden");
//       testStudent2.Save();
//
//       //Act
//       testCourse.AddStudent(testStudent);
//       testCourse.AddStudent(testStudent2);
//
//       List<Student> result = testCourse.GetStudents();
//       List<Student> testList = new List<Student>{testStudent, testStudent2};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_Delete_DeletesCourseAssociationsFromDatabase()
//     {
//       //Arrange
//       Student testStudent = new Student("Mow the lawn");
//       testStudent.Save();
//
//       string testName = "Home stuff";
//       Course testCourse = new Course(testName);
//       testCourse.Save();
//
//       //Act
//       testCourse.AddStudent(testStudent);
//       testCourse.Delete();
//
//       List<Course> resultStudentCourses = testStudent.GetCourses();
//       List<Course> testStudentCourses = new List<Course> {};
//
//       //Assert
//       Assert.Equal(testStudentCourses, resultStudentCourses);
//     }
//
    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
