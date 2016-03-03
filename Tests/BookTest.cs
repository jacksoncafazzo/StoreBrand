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
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
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
      Book firstBook = new Book("Life of the Past: Oregon", new DateTime(2013,2,12));
      Book secondBook = new Book("Life of the Past: Oregon", new DateTime(2013,2,12));

      //Assert
      Assert.Equal(firstBook, secondBook);
    }

    [Fact]
    public void Save_SavesBookToDatabase()
    {
       //Arrange
       Book testBook = new Book("Linguistical Problem Solving Programs", new DateTime(2011,9,10));
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
      Book testBook = new Book("Enterin' SQL Commands", new DateTime(2012,2,5));
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
      Book testBook = new Book("Screwin' Up Da'bases With Chris", new DateTime(2011,6,3));
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
      Book testBook = new Book("Best Book EVAH", new DateTime(2015,5,2));
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
      Author testAuthor = new Author("Perilous Dungeons", "sdlfk");
      testAuthor.Save();


      Book testBook = new Book("Wet doggie noses", new DateTime(1987,12,12));
      testBook.Save();

      //Act
      testBook.AddBookAuthor(testAuthor);
      testBook.Delete();

      List<Book> resultAuthorBooks = testAuthor.GetBooks();
      List<Book> testAuthorBooks = new List<Book> {};

      //Assert
      Assert.Equal(testAuthorBooks, resultAuthorBooks);
    }


    [Fact]
    public void AddBookAuthor_AddsAuthorToBook()
    {
      //Arrange
      Book testBook = new Book("David Copperfield", new DateTime(2014, 01, 01));
      testBook.Save();

      Author testAuthor = new Author ("Harry Henderson", "harryhenderson.com");
      testAuthor.Save();

      Author testAuthor2 = new Author ("Sally Henderson", "lilsal.com");
      testAuthor2.Save();

      //Act
      testBook.AddBookAuthor(testAuthor);
      testBook.AddBookAuthor(testAuthor2);

      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author>{testAuthor, testAuthor2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetAuthors_RetrievesAllAuthorsWithBook()
    {
      Book testBook = new Book("How It Be", new DateTime(2016, 03, 13));
      testBook.Save();

      Author firstAuthor = new Author("Pippi Longstocking", "sunnydays.vi.ca");
      firstAuthor.Save();
      testBook.AddBookAuthor(firstAuthor);

      Author secondAuthor = new Author("Matilda", "blah.com");
      secondAuthor.Save();
      testBook.AddBookAuthor(secondAuthor);

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
