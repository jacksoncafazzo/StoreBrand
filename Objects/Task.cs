using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
{
  private int _id;
  private string _name;
  private int _categoryId;

  public Task(string Name, int CategoryId, int Id = 0)
  {
    _id = Id;
    _name = Name;
    _categoryId = CategoryId;
  }

  public override bool Equals(System.Object otherTask)
  {
    if (!(otherTask is Task))
    {
      return false;
    }
    else
    {
      Task newTask = (Task) otherTask;
      bool idEquality = this.GetId() == newTask.GetId();
      bool nameEquality = this.GetName() == newTask.GetName();
      bool categoryEquality = this.GetCategoryId() == newTask.GetCategoryId();
      return (idEquality && nameEquality && categoryEquality);
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

    public int GetCategoryId()
  {
    return _categoryId;
  }
  public void SetCategoryId(int newCategoryId)
  {
    _categoryId = newCategoryId;
  }

  public static List<Task> GetAll()
  {
    List<Task> AllTasks = new List<Task>{};

    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM tasks;", conn);
    rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      int taskId = rdr.GetInt32(0);
      string taskName = rdr.GetString(1);
      int taskCategoryId = rdr.GetInt32(2);
      Task newTask = new Task(taskName, taskCategoryId, taskId);
      AllTasks.Add(newTask);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return AllTasks;
  }
  public void Save()
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr;
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO tasks (name, category_id) OUTPUT INSERTED.id VALUES (@TaskName, @TaskCategoryId);", conn);

    SqlParameter nameParameter = new SqlParameter();
    nameParameter.ParameterName = "@TaskName";
    nameParameter.Value = this.GetName();

    SqlParameter categoryIdParameter = new SqlParameter();
    categoryIdParameter.ParameterName = "@TaskCategoryId";
    categoryIdParameter.Value = this.GetCategoryId();

    cmd.Parameters.Add(nameParameter);
    cmd.Parameters.Add(categoryIdParameter);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskName = null;
      int foundTaskCategoryId = 0;

      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskName = rdr.GetString(1);
        foundTaskCategoryId = rdr.GetInt32(2);
      }
      Task foundTask = new Task(foundTaskName, foundTaskCategoryId, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }
    public override int GetHashCode()
    {
      return 0;
    }
  }
}
