using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HardKnockRegistrar
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arange, Act
      int result = Student.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Student firstStudent = new Student("Bob Marley", new DateTime (2016, 01, 01));
      Student secondStudent = new Student("Bob Marley", new DateTime (2016, 01, 01));

      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Student testStudent = new Student("Peter Tosh", new DateTime (2012, 01, 01));
      testStudent.Save();

      //Act
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Student testStudent = new Student("Bill Clinton", new DateTime (2016, 01, 01));
      testStudent.Save();

      //Act
      Student savedStudent = Student.GetAll()[0];

      int result = savedStudent.GetId();
      int testId = testStudent.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("Peter Griffin", new DateTime (2016, 01, 01));
      testStudent.Save();

      //Act
      Student result = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, result);
    }

    [Fact]
    public void Test_AddCourse_AddsCourseToStudent()
    {
      //Arrange
      Course testCourse = new Course("Evening TV", "NBC211");
      testCourse.Save();

      Student testStudent = new Student("Brian Griffin", new DateTime (2016, 01, 01));
      testStudent.Save();

      //Act
      testStudent.AddCourse(testCourse);

      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetCourses_ReturnsAllStudentCourses()
    {
      //Arrange
      Student testStudent = new Student("Mr. Ed", new DateTime(2054, 10, 12));
      testStudent.Save();

      Course testCourse1 = new Course("Mr. Magoo - Into the 22nd Century", "FMG240");
      testCourse1.Save();

      Course testCourse2 = new Course("Dreaming With Genie", "DJI300");
      testCourse2.Save();

      //Act
      testStudent.AddCourse(testCourse1);
      testStudent.AddCourse(testCourse2);
      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse1, testCourse2};

      //Assert
      Assert.Equal(testList, result);
    }

//     [Fact]
//     public void Test_Delete_DeletesStudentAssociationsFromDatabase()
//     {
//       //Arrange
//       Category testCategory = new Category("Home stuff");
//       testCategory.Save();
//
//       string testDescription = "Mow the lawn";
//       Student testStudent = new Student(testDescription);
//       testStudent.Save();
//
//       //Act
//       testStudent.AddCategory(testCategory);
//       testStudent.Delete();
//
//       List<Student> resultCategoryStudents = testCategory.GetStudents();
//       List<Student> testCategoryStudents = new List<Student> {};
//
//       //Assert
//       Assert.Equal(testCategoryStudents, resultCategoryStudents);
//     }
//
    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
