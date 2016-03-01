using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace HardKnockRegistrar
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _course_number;

    public Course(string Name, string CourseNumber, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _course_number = CourseNumber;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool nameEquality = this.GetName() == newCourse.GetName();
        bool numberEquality = this.GetCourseNumber() == newCourse.GetCourseNumber();
        return (idEquality && nameEquality && numberEquality);
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
    public string GetCourseNumber()
    {
      return _course_number;
    }
    public void SetCourseNumber(string newNumber)
    {
      _course_number = newNumber;
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(courseName, courseNumber, courseId);
        allCourses.Add(newCourse);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, course_number) OUTPUT INSERTED.id VALUES (@CourseName, @CourseNumber); INSERT INTO enrollments (course_id) OUTPUT INSERTED.id VALUES (@CourseId)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CourseName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      SqlParameter numberParameter = new SqlParameter();
      numberParameter.ParameterName = "@CourseNumber";
      numberParameter.Value = this.GetCourseNumber();
      cmd.Parameters.Add(numberParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();
      cmd.Parameters.Add(courseIdParameter);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseID;", conn);
      SqlParameter CourseIDParemeter = new SqlParameter();
      CourseIDParemeter.ParameterName = "@CourseId";
      CourseIDParemeter.Value = id.ToString();
      cmd.Parameters.Add(CourseIDParemeter);
      rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundCourseNumber = null;

      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
        foundCourseNumber = rdr.GetString(2);
      }
      Course foundCourse = new Course(foundCourseName, foundCourseNumber, foundCourseId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCourse;
    }

    public void AddEnrollment(Student newStudent)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO enrollments (course_id, student_id) VALUES (@CourseId, @StudentId)", conn);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();
      cmd.Parameters.Add(courseIdParameter);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = newStudent.GetId();
      cmd.Parameters.Add(studentIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN enrollments ON (courses.id = enrollments.course_id) JOIN students ON (enrollments.student_id = students.id) WHERE courses.id = @CourseId", conn);
      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();
      cmd.Parameters.Add(courseIdParameter);

      rdr = cmd.ExecuteReader();

      List<Student> students = new List<Student> {};
      int studentId = 0;
      string studentName = null;
      DateTime studentDate = new DateTime(2016, 01, 01);

      while(rdr.Read())
      {
        studentId = rdr.GetInt32(0);
        studentName = rdr.GetString(1);
        studentDate = rdr.GetDateTime(2);
        Student student = new Student(studentName, studentDate, studentId);
        students.Add(student);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return students;
    }
//
//       List<Student> students = new List<Student> {};
//       foreach (int studentId in studentIds)
//       {
//         SqlDataReader queryReader = null;
//         SqlCommand studentQuery = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
//
//         SqlParameter studentIdParameter = new SqlParameter();
//         studentIdParameter.ParameterName = "@StudentId";
//         studentIdParameter.Value = studentId;
//         studentQuery.Parameters.Add(studentIdParameter);
//
//         queryReader = studentQuery.ExecuteReader();
//         while(queryReader.Read())
//         {
//           int thisStudentId = queryReader.GetInt32(0);
//           string studentDescription = queryReader.GetString(1);
//           Student foundStudent = new Student(studentDescription, thisStudentId);
//           students.Add(foundStudent);
//         }
//         if (queryReader != null)
//         {
//           queryReader.Close();
//         }
//       }
      // if (conn != null)
      // {
      //   conn.Close();
      // }
//       return students;
//     }
//
    public void Delete()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("DELETE FROM courses WHERE id = @CourseId; DELETE FROM enrollments WHERE course_id = @CourseId;", conn);
     SqlParameter courseIdParameter = new SqlParameter();
     courseIdParameter.ParameterName = "@CourseId";
     courseIdParameter.Value = this.GetId();

     cmd.Parameters.Add(courseIdParameter);
     cmd.ExecuteNonQuery();

     if (conn != null)
     {
       conn.Close();
     }
   }
//
//     public void AddStudent(Student newStudent)
//     {
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId)", conn);
//       SqlParameter courseIdParameter = new SqlParameter();
//       courseIdParameter.ParameterName = "@CourseId";
//       courseIdParameter.Value = this.GetId();
//       cmd.Parameters.Add(courseIdParameter);
//
//       SqlParameter studentIdParameter = new SqlParameter();
//       studentIdParameter.ParameterName = "@StudentId";
//       studentIdParameter.Value = newStudent.GetId();
//       cmd.Parameters.Add(studentIdParameter);
//
//       cmd.ExecuteNonQuery();
//
//       if (conn != null)
//       {
//         conn.Close();
//       }
//     }
//
//     public override int GetHashCode()
//     {
//       return 0;
//     }

  }
}
