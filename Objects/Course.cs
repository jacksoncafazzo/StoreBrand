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

//     public void Save()
//     {
//       SqlConnection conn = DB.Connection();
//       SqlDataReader rdr;
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("INSERT INTO courses (name) OUTPUT INSERTED.id VALUES (@CourseName);", conn);
//
//       SqlParameter nameParameter = new SqlParameter();
//       nameParameter.ParameterName = "@CourseName";
//       nameParameter.Value = this.GetName();
//       cmd.Parameters.Add(nameParameter);
//       rdr = cmd.ExecuteReader();
//       while(rdr.Read())
//       {
//         this._id = rdr.GetInt32(0);
//       }
//       if (rdr != null)
//       {
//         rdr.Close();
//       }
//       if (conn != null)
//       {
//         conn.Close();
//       }
//     }
//
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
    }
//
//     public static Course Find(int id)
//     {
//       SqlConnection conn = DB.Connection();
//       SqlDataReader rdr = null;
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseID;", conn);
//       SqlParameter CourseIDParemeter = new SqlParameter();
//       CourseIDParemeter.ParameterName = "@CourseId";
//       CourseIDParemeter.Value = id.ToString();
//       cmd.Parameters.Add(CourseIDParemeter);
//       rdr = cmd.ExecuteReader();
//
//       int foundCourseId = 0;
//       string foundCourseDescription = null;
//
//       while(rdr.Read())
//       {
//         foundCourseId = rdr.GetInt32(0);
//         foundCourseDescription = rdr.GetString(1);
//       }
//       Course foundCourse = new Course(foundCourseDescription, foundCourseId);
//
//       if (rdr != null)
//       {
//         rdr.Close();
//       }
//       if (conn != null)
//       {
//         conn.Close();
//       }
//       return foundCourse;
//     }
//
//     public List<Task> GetTasks()
//     {
//       SqlConnection conn = DB.Connection();
//       SqlDataReader rdr = null;
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("SELECT task_id FROM courses_tasks WHERE course_id = @CourseId;", conn);
//       SqlParameter courseIdParameter = new SqlParameter();
//       courseIdParameter.ParameterName = "@CourseId";
//       courseIdParameter.Value = this.GetId();
//       cmd.Parameters.Add(courseIdParameter);
//
//       rdr = cmd.ExecuteReader();
//
//       List<int> taskIds = new List<int> {};
//       while(rdr.Read())
//       {
//         int taskId = rdr.GetInt32(0);
//         taskIds.Add(taskId);
//       }
//       if (rdr != null)
//       {
//         rdr.Close();
//       }
//
//       List<Task> tasks = new List<Task> {};
//       foreach (int taskId in taskIds)
//       {
//         SqlDataReader queryReader = null;
//         SqlCommand taskQuery = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
//
//         SqlParameter taskIdParameter = new SqlParameter();
//         taskIdParameter.ParameterName = "@TaskId";
//         taskIdParameter.Value = taskId;
//         taskQuery.Parameters.Add(taskIdParameter);
//
//         queryReader = taskQuery.ExecuteReader();
//         while(queryReader.Read())
//         {
//           int thisTaskId = queryReader.GetInt32(0);
//           string taskDescription = queryReader.GetString(1);
//           Task foundTask = new Task(taskDescription, thisTaskId);
//           tasks.Add(foundTask);
//         }
//         if (queryReader != null)
//         {
//           queryReader.Close();
//         }
//       }
//       if (conn != null)
//       {
//         conn.Close();
//       }
//       return tasks;
//     }
//
//     public void Delete()
//    {
//      SqlConnection conn = DB.Connection();
//      conn.Open();
//
//      SqlCommand cmd = new SqlCommand("DELETE FROM courses WHERE id = @CourseId; DELETE FROM courses_tasks WHERE course_id = @CourseId;", conn);
//      SqlParameter courseIdParameter = new SqlParameter();
//      courseIdParameter.ParameterName = "@CourseId";
//      courseIdParameter.Value = this.GetId();
//
//      cmd.Parameters.Add(courseIdParameter);
//      cmd.ExecuteNonQuery();
//
//      if (conn != null)
//      {
//        conn.Close();
//      }
//    }
//
//     public void AddTask(Task newTask)
//     {
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("INSERT INTO courses_tasks (course_id, task_id) VALUES (@CourseId, @TaskId)", conn);
//       SqlParameter courseIdParameter = new SqlParameter();
//       courseIdParameter.ParameterName = "@CourseId";
//       courseIdParameter.Value = this.GetId();
//       cmd.Parameters.Add(courseIdParameter);
//
//       SqlParameter taskIdParameter = new SqlParameter();
//       taskIdParameter.ParameterName = "@TaskId";
//       taskIdParameter.Value = newTask.GetId();
//       cmd.Parameters.Add(taskIdParameter);
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
