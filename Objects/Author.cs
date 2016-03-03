using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
  public class Author
  {
    private int _id;
    private string _firstName;
    private string _lastName;

    public Author(string FirstName, string LastName, int Id = 0)
    {
      _id = Id;
      _firstName = FirstName;
      _lastName = LastName;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else {
        Author newAuthor = (Author) otherAuthor;
        bool IdEquality = this.GetId() == newAuthor.GetId();
        bool FirstNameEquality = this.GetFirstName() == newAuthor.GetFirstName();
        bool LastNameEquality = this.GetLastName() == newAuthor.GetLastName();

        return (IdEquality && FirstNameEquality && LastNameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetFirstName()
    {
      return _firstName;
    }
    public void SetName(string newName)
    {
      _firstName = newName;
    }
    public String GetLastName()
    {
      return _lastName;
    }
    public void SetLastName(string newLastName)
    {
      _lastName = newLastName;
    }

    public static List<Author> GetAll()
    {
      List<Author> AllAuthors = new List<Author>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int AuthorId = rdr.GetInt32(0);
        string AuthorFirstName = rdr.GetString(1);
        string AuthorLastName = rdr.GetString(2);
        Author NewAuthor = new Author(AuthorFirstName, AuthorLastName, AuthorId);
        AllAuthors.Add(NewAuthor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllAuthors;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (firstname, lastname) OUTPUT INSERTED.id VALUES (@FirstName, @LastName); INSERT INTO book_author (author_id) OUTPUT INSERTED.id VALUES (@AuthorId);", conn);

      SqlParameter firstNameParam = new SqlParameter();
      firstNameParam.ParameterName = "@FirstName";
      firstNameParam.Value = this.GetFirstName();
      cmd.Parameters.Add(firstNameParam);

      SqlParameter lastNameParam = new SqlParameter();
      lastNameParam.ParameterName = "@LastName";
      lastNameParam.Value = this.GetLastName();
      cmd.Parameters.Add(lastNameParam);

      SqlParameter authorIdParam = new SqlParameter();
      authorIdParam.ParameterName = "@AuthorId";
      authorIdParam.Value = this.GetId();
      cmd.Parameters.Add(authorIdParam);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Author Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @AuthorId", conn);
      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = id.ToString();
      cmd.Parameters.Add(authorIdParameter);
      rdr = cmd.ExecuteReader();

      int foundAuthorId = 0;
      string foundFirstName = null;
      string foundLastName = null;

      while(rdr.Read())
      {
        foundAuthorId = rdr.GetInt32(0);
        foundFirstName = rdr.GetString(1);
        foundLastName = rdr.GetString(2);
      }
      Author foundAuthor = new Author(foundFirstName, foundLastName, foundAuthorId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAuthor;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors WHERE id = @AuthorId; DELETE FROM book_author WHERE author_id = @AuthorId;", conn);
      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();

      cmd.Parameters.Add(authorIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void AddBook(Book newBook)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO book_author (book_id, author_id) VALUES (@BookId, @AuthorId)", conn);  //needs more stuff - book relationship
      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = newBook.GetId();
      cmd.Parameters.Add(bookIdParameter);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();
      cmd.Parameters.Add(authorIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Book> GetBooks()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN book_author ON (authors.id = book_author.author_id) JOIN books ON (book_author.book_id = books.id) WHERE authors.id = @AuthorId", conn);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();
      cmd.Parameters.Add(authorIdParameter);

      rdr = cmd.ExecuteReader();

      List<Book> books = new List<Book> {};
      int bookId = 0;
      string bookTitle = null;
      DateTime publishDate = new DateTime(2000, 1, 1);

      while (rdr.Read())
      {
        bookId = rdr.GetInt32(0);
        bookTitle = rdr.GetString(1);
        publishDate = rdr.GetDateTime(2);
        Book book = new Book(bookTitle, publishDate, bookId);
        books.Add(book);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      return books;
    }
  }
}
