using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
  public class Book
  {
    private int _id;
    private string _title;
    private DateTime _due_date;

    public Book(string Title, DateTime DueDate, int Id = 0)
    {
      _id = Id;
      _title = Title;
      _due_date = DueDate;
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
        bool dueDateEquality = this.GetDueDate() == newBook.GetDueDate();
        return (idEquality && titleEquality && dueDateEquality);
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
    public string GetDueDate()
    {
      return _due_date;
    }
    public void SetDueDate(string newNumber)
    {
      _due_date = newNumber;
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
        string bookNumber = rdr.GetString(2);
        Book newBook = new Book(bookTitle, bookNumber, bookId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title, due_date) OUTPUT INSERTED.id VALUES (@BookName, @DueDate); INSERT INTO book_author (book_id) OUTPUT INSERTED.id VALUES (@BookId)", conn);

      SqlParameter titleParameter = new SqlParameter();
      titleParameter.ParameterName = "@BookName";
      titleParameter.Value = this.GetName();
      cmd.Parameters.Add(titleParameter);

      SqlParameter dueDateParameter = new SqlParameter();
      dueDateParameter.ParameterName = "@DueDate";
      dueDateParameter.Value = this.GetDueDate();
      cmd.Parameters.Add(dueDateParameter);

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
      string foundDueDate = null;

      while(rdr.Read())
      {
        foundBookId = rdr.GetInt32(0);
        foundBookTitle = rdr.GetString(1);
        foundDueDate = rdr.GetString(2);
      }
      Book foundBook = new Book(foundBookTitle, foundDueDate, foundBookId);

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
      string authorName = null;
      DateTime authorDate = new DateTime(2016, 01, 01);

      while(rdr.Read())
      {
        authorId = rdr.GetInt32(0);
        authorName = rdr.GetString(1);
        authorDate = rdr.GetDateTime(2);
        Author author = new Author(authorName, authorDate, authorId);
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
