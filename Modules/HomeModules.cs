using Nancy;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace LibraryCatalog
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

      Get["/authors"] = _ => {
        List<Author> AllAuthors = Author.GetAll();
        return View["authors.cshtml", AllAuthors];
      };

      Get["/books"] = _ => {
        List<Book> AllBooks = Book.GetAll();
        return View["books.cshtml", AllBooks];
      };

      Get["/authors/new"] = _ => {
        return View["authors_form.cshtml"];
      };

      Post["/authors/new"] = _ => {
        Author newAuthor = new Author(Request.Form["author-name"], Request.Form["book-author"]);
        newAuthor.Save();
        List<Author> AllAuthors = Author.GetAll();
        return View["authors.cshtml", AllAuthors];
      };

      Get["/books/new"] = _ => {
        return View["books_form.cshtml"];
      };

      Post["/books/new"] = _ => {
        Book newBook = new Book(Request.Form["book-name"], Request.Form["book-number"]);
        newBook.Save();
        List<Book> AllBooks = Book.GetAll();
        return View["books.cshtml", AllBooks];
      };


    }
  }
}
