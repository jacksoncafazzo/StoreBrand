using Nancy;
using ToDoList;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace HardKnockRegistrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

      Get["/students"] = _ => {
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };

      Get["/courses"] = _ => {
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };

      Get["/students/new"] = _ => {
        return View["students_form.cshtml"];
      };

      Post["/students/new"] = _ => {
        Student newStudent = new Student(Request.Form["student-name"], Request.Form["enrollment-date"]);
        newStudent.Save();
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };

      Get["/courses/new"] = _ => {
        return View["courses_form.cshtml"];
      };

      Post["/courses/new"] = _ => {
        Course newCourse = new Course(Request.Form["course-name"], Request.Form["course-number"]);
        newCourse.Save();
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };


    }
  }
}
