using Nancy;
using System;
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

      Post["/authors"] = _ => {
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
        Author newAuthor = new Author(Request.Form["first-name"], Request.Form["last-name"]);
        newAuthor.Save();
        List<Author> AllAuthors = Author.GetAll();
        return View["authors.cshtml", AllAuthors];
      };

      Get["/authors/{id}"] = parameters => {
        Author newAuthor = Author.Find(parameters.id);
        List<Author> AllAuthors = new List<Author>{};
        AllAuthors.Add(newAuthor);
        return View["author.cshtml", AllAuthors];
      };

      Delete["/authors/{id}"] = parameters => {
        Author newAuthor = Author.Find(parameters.id);
        newAuthor.Delete();
        List<Author> AllAuthors = Author.GetAll();
        return View["author.cshtml", AllAuthors];
      };

      Get["/books/new"] = _ => {
        return View["books_form.cshtml"];
      };

      Post["/books/new"] = _ => {
        DateTime pubTime = Convert.ToDateTime((string)Request.Form["book-pub"]);
        Book newBook = new Book(Request.Form["book-title"], pubTime);
        newBook.Save();
        Author newAuthor = new Author(Request.Form["book-author-fn"], Request.Form["book-author-ln"]);
        newBook.AddBookAuthor(newAuthor);
        List<Book> AllBooks = Book.GetAll();
        return View["books.cshtml", AllBooks];
      };

      Get["/books/{id}"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Book newBook = Book.Find(parameters.id);
        List<Author> selectedAuthors = newBook.GetAuthors();
        //add copies
        models.Add("authors", selectedAuthors);
        models.Add("book", newBook);
        return View["book.cshtml", models];
      };

      Delete["/books/delete"] = _ => {
        Book.DeleteAll();
        return View["index.cshtml"];
      };

      Delete["/authors/delete"] = _ => {
        Author.DeleteAll();
        return View["index.cshtml"];
      };


    }
  }
}
