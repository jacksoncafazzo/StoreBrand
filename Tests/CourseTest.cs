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

    [Fact]
    public void Test_AddEnrollment_AddsStudentToCourse()
    {
      //Arrange
      Course testCourse = new Course("David Copperfield", "MAG120");
      testCourse.Save();

      Student testStudent = new Student ("Harry Henderson", new DateTime(2014, 01, 01));
      testStudent.Save();

      Student testStudent2 = new Student ("Sally Henderson", new DateTime(2014, 01, 01));
      testStudent2.Save();

      //Act
      testCourse.AddEnrollment(testStudent);
      testCourse.AddEnrollment(testStudent2);

      List<Student> result = testCourse.GetStudents();
      List<Student> testList = new List<Student>{testStudent, testStudent2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetStudents_RetrievesAllStudentsWithCourse()
    {
      Course testCourse = new Course("How It Be", "HIB500");
      testCourse.Save();

      Student firstStudent = new Student("Pippi Longstocking", new DateTime(2016, 03, 13));
      firstStudent.Save();
      testCourse.AddEnrollment(firstStudent);

      Student secondStudent = new Student("Matilda", new DateTime(2016, 02, 13));
      secondStudent.Save();
      testCourse.AddEnrollment(secondStudent);

      List<Student> testStudentList = new List<Student> {firstStudent, secondStudent};
      List<Student> resultStudentList = testCourse.GetStudents();

      Assert.Equal(testStudentList, resultStudentList);
    }

    [Fact]
    public void Test_Delete_DeletesCourseFromDatabase()
    {
      List<Course> resultCourses = Course.GetAll();
      //Arrange
      Course testCourse = new Course("Best Course EVAH", "BCE500");
      testCourse.Save();
      testCourse.Delete();

      List<Course> otherResultCourses = Course.GetAll();

      //Assert
      Assert.Equal(otherResultCourses, resultCourses);
    }


    [Fact]
    public void Test_Delete_DeletesCourseEnrollmentsFromDatabase()
    {
      //Arrange
      Student testStudent = new Student("Sappy Sara", new DateTime(2010, 10, 12));
      testStudent.Save();


      Course testCourse = new Course("Wet doggie noses", "WDN100");
      testCourse.Save();

      //Act
      testCourse.AddEnrollment(testStudent);
      testCourse.Delete();

      List<Course> resultStudentCourses = testStudent.GetCourses();
      List<Course> testStudentCourses = new List<Course> {};

      //Assert
      Assert.Equal(testStudentCourses, resultStudentCourses);
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
