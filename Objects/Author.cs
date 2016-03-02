using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
  public class Author
  {
    private int _id;
    private string _name;
    private string _url;

    public Author(string Name, string Url, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _url = Url;
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
        bool NameEquality = this.GetName() == newAuthor.GetName();
        bool UrlEquality = this.GetUrl() == newAuthor.GetUrl();

        return (IdEquality && NameEquality && UrlEquality);
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
    public String GetUrl()
    {
      return _url;
    }
    public void SetUrl(string newUrl)
    {
      _url = newUrl;
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
        string AuthorName = rdr.GetString(1);
        string AuthorUrl = rdr.GetString(2);
        Author NewAuthor = new Author(AuthorName, AuthorUrl, AuthorId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (name, url) OUTPUT INSERTED.id VALUES (@AuthorName, @Url); INSERT INTO book_authors (author_id) OUTPUT INSERTED.id VALUES (@AuthorId);", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@AuthorName";
      nameParam.Value = this.GetName();
      cmd.Parameters.Add(nameParam);

      SqlParameter dateParam = new SqlParameter();
      dateParam.ParameterName = "@Url";
      dateParam.Value = this.GetUrl();
      cmd.Parameters.Add(dateParam);

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
      string foundAuthorName = null;
      string foundUrl = null;

      while(rdr.Read())
      {
        foundAuthorId = rdr.GetInt32(0);
        foundAuthorName = rdr.GetString(1);
        foundUrl = rdr.GetString(2);
      }
      Author foundAuthor = new Author(foundAuthorName, foundUrl, foundAuthorId);

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

      SqlCommand cmd = new SqlCommand("DELETE FROM authors WHERE id = @AuthorId; DELETE FROM book_authors WHERE author_id = @AuthorId;", conn);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO book_authors (book_id, author_id) VALUES (@BookId, @AuthorId)", conn);
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

      SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN book_authors ON (authors.id = book_authors.author_id) JOIN books ON (book_authors.book_id = books.id) WHERE authors.id = @AuthorId", conn);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();
      cmd.Parameters.Add(authorIdParameter);

      rdr = cmd.ExecuteReader();

      List<Book> books = new List<Book> {};
      int bookId = 0;
      string bookName = null;
      string bookNumber = null;

      while (rdr.Read())
      {
        bookId = rdr.GetInt32(0);
        bookName = rdr.GetString(1);
        bookNumber = rdr.GetString(2);
        Book book = new Book(bookName, bookNumber, bookId);
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
