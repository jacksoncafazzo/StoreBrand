using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCatalog
{
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void DatabaseEmptyAtFirst()
    {
      //Arange, Act
      int result = Author.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Author firstAuthor = new Author("Bob Marley", "bobmarley.com");
      Author secondAuthor = new Author("Bob Marley", "bobmarley.com");

      //Assert
      Assert.Equal(firstAuthor, secondAuthor);
    }

    [Fact]
    public void Save()
    {
      //Arrange
      Author testAuthor = new Author("Peter Tosh", "petertosh.com");
      testAuthor.Save();

      //Act
      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void SaveAssignsIdToObject()
    {
      //Arrange
      Author testAuthor = new Author("Bill Clinton", "billclinton.com");
      testAuthor.Save();

      //Act
      Author savedAuthor = Author.GetAll()[0];

      int result = savedAuthor.GetId();
      int testId = testAuthor.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void FindFindsAuthorInDatabase()
    {
      //Arrange
      Author testAuthor = new Author("Peter Griffin", "familyguy.com");
      testAuthor.Save();

      //Act
      Author result = Author.Find(testAuthor.GetId());

      //Assert
      Assert.Equal(testAuthor, result);
    }

    [Fact]
    public void AddBook_AddsBookToAuthor()
    {
      //Arrange
      Book testBook = new Book("Evening TV", new DateTime (2016, 01, 01));
      testBook.Save();

      Author testAuthor = new Author("Brian Griffin", "familyguy.com");
      testAuthor.Save();

      //Act
      testAuthor.AddBook(testBook);

      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetBooks_ReturnsAllAuthorBooks()
    {
      //Arrange
      Author testAuthor = new Author("Mr. Ed", "nickatnite.com");
      testAuthor.Save();

      Book testBook1 = new Book("Mr. Magoo - Into the 22nd Century", new DateTime(2, 10, 12));
      testBook1.Save();

      Book testBook2 = new Book("Dreaming With Genie", new DateTime(2, 8, 12));
      testBook2.Save();

      //Act
      testAuthor.AddBook(testBook1);
      testAuthor.AddBook(testBook2);
      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book> {testBook1, testBook2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesAuthorBookAuthorsFromDatabase()
    {
      //Arrange
      Book testBook = new Book("Bees and their Stingers", new DateTime(2, 10, 12));
      testBook.Save();

      Author testAuthor = new Author("Farmer John", "Killer Mike");
      testAuthor.Save();

      //Act
      testAuthor.AddBook(testBook);
      testAuthor.Delete();

      List<Author> resultBookAuthors = testBook.GetAuthors();
      List<Author> testBookAuthors = new List<Author> {};

      //Assert
      Assert.Equal(testBookAuthors, resultBookAuthors);
    }

    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
