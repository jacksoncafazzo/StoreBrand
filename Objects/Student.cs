using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace HardKnockRegistrar
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _dateOfEnrollment;

    public Student(string Name, DateTime DateOfEnrollment, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _dateOfEnrollment = DateOfEnrollment;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else {
        Student newStudent = (Student) otherStudent;
        bool IdEquality = this.GetId() == newStudent.GetId();
        bool NameEquality = this.GetName() == newStudent.GetName();
        bool DateOfEnrollmentEquality = this.GetDateOfEnrollment() == newStudent.GetDateOfEnrollment();

        return (IdEquality && NameEquality && DateOfEnrollmentEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public DateTime GetDateOfEnrollment()
    {
      return _dateOfEnrollment;
    }
    public void SetDateOfEnrollment(DateTime newDateOfEnrollment)
    {
      _dateOfEnrollment = newDateOfEnrollment;
    }

    public static List<Student> GetAll()
    {
      List<Student> AllStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int StudentId = rdr.GetInt32(0);
        string StudentName = rdr.GetString(1);
        DateTime StudentDateOfEnrollment = rdr.GetDateTime(2);
        Student NewStudent = new Student(StudentName, StudentDateOfEnrollment, StudentId);
        AllStudents.Add(NewStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllStudents;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (name, date_of_enrollment) OUTPUT INSERTED.id VALUES (@StudentName, @DateOfEnrollment); INSERT INTO enrollments (student_id) OUTPUT INSERTED.id VALUES (@StudentId);", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@StudentName";
      nameParam.Value = this.GetName();
      cmd.Parameters.Add(nameParam);

      SqlParameter dateParam = new SqlParameter();
      dateParam.ParameterName = "@DateOfEnrollment";
      dateParam.Value = this.GetDateOfEnrollment();
      cmd.Parameters.Add(dateParam);

      SqlParameter studentIdParam = new SqlParameter();
      studentIdParam.ParameterName = "@StudentId";
      studentIdParam.Value = this.GetId();
      cmd.Parameters.Add(studentIdParam);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(studentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime foundDateOfEnrollment = new DateTime(2016, 01, 01);

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundDateOfEnrollment = rdr.GetDateTime(2);
      }
      Student foundStudent = new Student(foundStudentName, foundDateOfEnrollment, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }

  //   public void Delete()
  //   {
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM categories_students WHERE student_id = @StudentId;", conn);
  //     SqlParameter studentIdParameter = new SqlParameter();
  //     studentIdParameter.ParameterName = "@StudentId";
  //     studentIdParameter.Value = this.GetId();
  //
  //     cmd.Parameters.Add(studentIdParameter);
  //     cmd.ExecuteNonQuery();
  //
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //   }
  //
  //   public void AddCategory(Category newCategory)
  //   {
  //     SqlConnection conn = DB.Connection();
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("INSERT INTO categories_students (category_id, student_id) VALUES (@CategoryId, @StudentId);", conn);
  //
  //     SqlParameter categoryIdParameter = new SqlParameter();
  //     categoryIdParameter.ParameterName = "@CategoryId";
  //     categoryIdParameter.Value = newCategory.GetId();
  //     cmd.Parameters.Add(categoryIdParameter);
  //
  //     SqlParameter studentIdParameter = new SqlParameter();
  //     studentIdParameter.ParameterName = "@StudentId";
  //     studentIdParameter.Value = this.GetId();
  //     cmd.Parameters.Add(studentIdParameter);
  //
  //     cmd.ExecuteNonQuery();
  //
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //   }
  //
  //   public List<Category> GetCategories()
  //   {
  //     SqlConnection conn = DB.Connection();
  //     SqlDataReader rdr = null;
  //     conn.Open();
  //
  //     SqlCommand cmd = new SqlCommand("SELECT category_id FROM categories_students WHERE student_id = @StudentId;", conn);
  //
  //     SqlParameter studentIdParameter = new SqlParameter();
  //     studentIdParameter.ParameterName = "@StudentId";
  //     studentIdParameter.Value = this.GetId();
  //     cmd.Parameters.Add(studentIdParameter);
  //
  //     rdr = cmd.ExecuteReader();
  //
  //     List<int> categoryIds = new List<int> {};
  //
  //     while (rdr.Read())
  //     {
  //       int categoryId = rdr.GetInt32(0);
  //       categoryIds.Add(categoryId);
  //     }
  //     if (rdr != null)
  //     {
  //       rdr.Close();
  //     }
  //
  //     List<Category> categories = new List<Category> {};
  //
  //     foreach (int categoryId in categoryIds)
  //     {
  //       SqlDataReader queryReader = null;
  //       SqlCommand categoryQuery = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);
  //
  //       SqlParameter categoryIdParameter = new SqlParameter();
  //       categoryIdParameter.ParameterName = "@CategoryId";
  //       categoryIdParameter.Value = categoryId;
  //       categoryQuery.Parameters.Add(categoryIdParameter);
  //
  //       queryReader = categoryQuery.ExecuteReader();
  //       while (queryReader.Read())
  //       {
  //         int thisCategoryId = queryReader.GetInt32(0);
  //         string categoryName = queryReader.GetString(1);
  //         Category foundCategory = new Category(categoryName, thisCategoryId);
  //         categories.Add(foundCategory);
  //       }
  //       if (queryReader != null)
  //       {
  //         queryReader.Close();
  //       }
  //     }
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //     return categories;
  //   }
  }
}
