using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCatalog
{
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_BooksEmptyAtFirst()
    {
      //Arrange, Act
      int result = Book.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Book firstBook = new Book("Life of the Past: Oregon", new DateTime(3,2,2013);
      Book secondBook = new Book("Life of the Past: Oregon", new DateTime(3,2,2013);

      //Assert
      Assert.Equal(firstBook, secondBook);
    }

    [Fact]
    public void Save_SavesBookToDatabase()
    {
       //Arrange
       Book testBook = new Book("Linguistical Problem Solving Programs", new DateTime(4,20,2013));
       testBook.Save();

       //Act
       List<Book> result = Book.GetAll();
       List<Book> testList = new List<Book>{testBook};

       //Assert
       Assert.Equal(testList, result);
    }
//
    [Fact]
    public void Save_AssignsIdToBookObject()
    {
      //Arrange
      Book testBook = new Book("Enterin' SQL Commands", new DateTime(5,7,2012));
      testBook.Save();

      //Act
      Book savedBook = Book.GetAll()[0];

      int result = savedBook.GetId();
      int testId = testBook.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Find_FindsBookInDatabase()
    {
      //Arrange
      Book testBook = new Book("Screwin' Up Da'bases With Chris", new DateTime(6,6,2011));
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.GetId());

      //Assert
      Assert.Equal(testBook, foundBook);
    }

    [Fact]
    public void Delete_DeletesBookFromDatabase()
    {
      List<Book> resultBooks = Book.GetAll();
      //Arrange
      Book testBook = new Book("Best Book EVAH", new DateTime(11,6,2015));
      testBook.Save();
      testBook.Delete();

      List<Book> otherResultBooks = Book.GetAll();

      //Assert
      Assert.Equal(otherResultBooks, resultBooks);
    }

    [Fact]
    public void Delete_DeletesBookBookAndAuthorsFromDatabase()
    {
      //Arrange
      Author testAuthor = new Author("Perilous Dungeons", new DateTime()));
      testAuthor.Save();


      Book testBook = new Book("Wet doggie noses", new DateTime(5,12,1987));
      testBook.Save();

      //Act
      testBook.AddAuthorToBook(testAuthor);
      testBook.Delete();

      List<Book> resultAuthorBooks = testAuthor.GetBooks();
      List<Book> testAuthorBooks = new List<Book> {};

      //Assert
      Assert.Equal(testAuthorBooks, resultAuthorBooks);
    }


    [Fact]
    public void AddAuthorToBook_AddsAuthorToBook()
    {
      //Arrange
      Book testBook = new Book("David Copperfield", new DateTime(2014, 01, 01));
      testBook.Save();

      Author testAuthor = new Author ("Harry Henderson", "harryhenderson.com");
      testAuthor.Save();

      Author testAuthor2 = new Author ("Sally Henderson", "lilsal.com");
      testAuthor2.Save();

      //Act
      testBook.AddAuthorToBook(testAuthor);
      testBook.AddAuthorToBook(testAuthor2);

      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author>{testAuthor, testAuthor2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetAuthors_RetrievesAllAuthorsWithBook()
    {
      Book testBook = new Book("How It Be", DateTime(2016, 03, 13));
      testBook.Save();

      Author firstAuthor = new Author("Pippi Longstocking", "sunnydays.vi.ca");
      firstAuthor.Save();
      testBook.AddAuthorToBook(firstAuthor);

      Author secondAuthor = new Author("Matilda", "blah.com");
      secondAuthor.Save();
      testBook.AddAuthorToBook(secondAuthor);

      List<Author> testAuthorList = new List<Author> {firstAuthor, secondAuthor};
      List<Author> resultAuthorList = testBook.GetAuthors();

      Assert.Equal(testAuthorList, resultAuthorList);
    }

    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
