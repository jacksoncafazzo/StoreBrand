using Nancy;
using ToDoList;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

    }
  }
}
