using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
  public class Book
  {
    private int _id;
    private string _title;
    private DateTime _publish_date;

    public Book(string Title, DateTime PublishDate, int Id = 0)
    {
      _id = Id;
      _title = Title;
      _publish_date = PublishDate;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = this.GetId() == newBook.GetId();
        bool titleEquality = this.GetTitle() == newBook.GetTitle();
        bool publishDateEquality = this.GetPublishDate() == newBook.GetPublishDate();
        return (idEquality && titleEquality && publishDateEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
    return _title;
    }
    public void SetTitle(string newTitle)
    {
      _title = newTitle;
    }
    public DateTime GetPublishDate()
    {
      return _publish_date;
    }
    public void SetPublishDate(DateTime newNumber)
    {
      _publish_date = newNumber;
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        DateTime PublishDate = rdr.GetDateTime(2);
        Book newBook = new Book(bookTitle, PublishDate, bookId);
        allBooks.Add(newBook);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allBooks;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title, publish_date) OUTPUT INSERTED.id VALUES (@BookTitle, @PublishDate); INSERT INTO book_author (book_id) OUTPUT INSERTED.id VALUES (@BookId)", conn);

      SqlParameter titleParameter = new SqlParameter();
      titleParameter.ParameterName = "@BookTitle";
      titleParameter.Value = this.GetTitle();
      cmd.Parameters.Add(titleParameter);

      SqlParameter publishDateParameter = new SqlParameter();
      publishDateParameter.ParameterName = "@PublishDate";
      publishDateParameter.Value = this.GetPublishDate();
      cmd.Parameters.Add(publishDateParameter);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bookIdParameter);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Book Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @BookID;", conn);
      SqlParameter BookIDParemeter = new SqlParameter();
      BookIDParemeter.ParameterName = "@BookId";
      BookIDParemeter.Value = id.ToString();
      cmd.Parameters.Add(BookIDParemeter);
      rdr = cmd.ExecuteReader();

      int foundBookId = 0;
      string foundBookTitle = null;
      DateTime foundPublishDate = new DateTime(2000,1,1);

      while(rdr.Read())
      {
        foundBookId = rdr.GetInt32(0);
        foundBookTitle = rdr.GetString(1);
        foundPublishDate = rdr.GetDateTime(2);
      }
      Book foundBook = new Book(foundBookTitle, foundPublishDate, foundBookId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBook;
    }

    public void AddBookAuthor(Author newAuthor)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO book_author (book_id, author_id) VALUES (@BookId, @AuthorId)", conn);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bookIdParameter);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = newAuthor.GetId();
      cmd.Parameters.Add(authorIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Author> GetAuthors()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN book_author ON (books.id = book_author.book_id) JOIN authors ON (book_author.author_id = authors.id) WHERE books.id = @BookId", conn);
      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bookIdParameter);

      rdr = cmd.ExecuteReader();

      List<Author> authors = new List<Author> {};
      int authorId = 0;
      string authorFirstName = null;
      string authorLastName = null;

      while(rdr.Read())
      {
        authorId = rdr.GetInt32(0);
        authorFirstName = rdr.GetString(1);
        authorLastName = rdr.GetString(2);
        Author author = new Author(authorFirstName, authorLastName, authorId);
        authors.Add(author);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return authors;
    }

    public void Delete()
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @BookId; DELETE FROM book_author WHERE book_id = @BookId;", conn);
       SqlParameter bookIdParameter = new SqlParameter();
       bookIdParameter.ParameterName = "@BookId";
       bookIdParameter.Value = this.GetId();

       cmd.Parameters.Add(bookIdParameter);
       cmd.ExecuteNonQuery();

       if (conn != null)
       {
         conn.Close();
       }
     }

    public override int GetHashCode()
    {
      return 0;
    }

  }
}
