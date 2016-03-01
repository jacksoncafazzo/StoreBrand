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


//     [Fact]
//     public void Test_EmptyAtFirst()
//     {
//       //Arrange, Act
//       int result = Student.GetAll().Count;
//
//       //Assert
//       Assert.Equal(0, result);
//     }
//
//     [Fact]
//     public void Test_Save()
//     {
//       //Arrange
//       Student testStudent = new Student("Mow the lawn");
//       testStudent.Save();
//
//       //Act
//       List<Student> result = Student.GetAll();
//       List<Student> testList = new List<Student>{testStudent};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_SaveAssignsIdToObject()
//     {
//       //Arrange
//       Student testStudent = new Student("Mow the lawn");
//       testStudent.Save();
//
//       //Act
//       Student savedStudent = Student.GetAll()[0];
//
//       int result = savedStudent.GetId();
//       int testId = testStudent.GetId();
//
//       //Assert
//       Assert.Equal(testId, result);
//     }
//
//     [Fact]
//     public void Test_FindFindsStudentInDatabase()
//     {
//       //Arrange
//       Student testStudent = new Student("Mow the lawn");
//       testStudent.Save();
//
//       //Act
//       Student result = Student.Find(testStudent.GetId());
//
//       //Assert
//       Assert.Equal(testStudent, result);
//     }
//
//     [Fact]
//     public void Test_AddCategory_AddsCategoryToStudent()
//     {
//       //Arrange
//       Student testStudent = new Student("Mow the lawn");
//       testStudent.Save();
//
//       Category testCategory = new Category("Home stuff");
//       testCategory.Save();
//
//       //Act
//       testStudent.AddCategory(testCategory);
//
//       List<Category> result = testStudent.GetCategories();
//       List<Category> testList = new List<Category>{testCategory};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_GetCategories_ReturnsAllStudentCategories()
//     {
//       //Arrange
//       Student testStudent = new Student("Mow the lawn");
//       testStudent.Save();
//
//       Category testCategory1 = new Category("Home stuff");
//       testCategory1.Save();
//
//       Category testCategory2 = new Category("Work stuff");
//       testCategory2.Save();
//
//       //Act
//       testStudent.AddCategory(testCategory1);
//       List<Category> result = testStudent.GetCategories();
//       List<Category> testList = new List<Category> {testCategory1};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
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
      // Category.DeleteAll();
    }
  }
}
