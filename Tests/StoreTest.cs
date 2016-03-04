using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StoreBrand
{
  public class StoreTest : IDisposable
  {
    public StoreTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=storebrand_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_StoresEmptyAtFirst()
    {
      //Arrange, Act
      int result = Store.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Store firstStore = new Store("PayLess Shoes", "payless.com");
      Store secondStore = new Store("PayLess Shoes", "payless.com");

      //Assert
      Assert.Equal(firstStore, secondStore);
    }

    [Fact]
    public void Save_SavesStoreToDatabase()
    {
       //Arrange
       Store testStore = new Store("Ross","ross.com");
       testStore.Save();

       //Act
       List<Store> result = Store.GetAll();
       List<Store> testList = new List<Store>{testStore};

       //Assert
       Assert.Equal(testList, result);
    }
//
    [Fact]
    public void Save_AssignsIdToStoreObject()
    {
      //Arrange
      Store testStore = new Store("John Flugvog", "flugvog.com");
      testStore.Save();

      //Act
      Store savedStore = Store.GetAll()[0];

      int result = savedStore.GetId();
      int testId = testStore.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Find_FindsStoreInDatabase()
    {
      //Arrange
      Store testStore = new Store("Goodwill", "goodwill.com");
      testStore.Save();

      //Act
      Store foundStore = Store.Find(testStore.GetId());

      //Assert
      Assert.Equal(testStore, foundStore);
    }

    [Fact]
    public void Delete_DeletesStoreFromDatabase()
    {
      List<Store> resultStores = Store.GetAll();
      //Arrange
      Store testStore = new Store("Best Store EVAH", "beststoreevah.com");
      testStore.Save();
      testStore.Delete();

      List<Store> otherResultStores = Store.GetAll();

      //Assert
      Assert.Equal(otherResultStores, resultStores);
    }

    [Fact]
    public void Delete_DeletesStoreStoreAndBrandsFromDatabase()
    {
      //Arrange
      Brand testBrand = new Brand("Nike", "http://nike.com/logo.png");
      testBrand.Save();


      Store testStore = new Store("Keen Garage", "keen.com");
      testStore.Save();

      //Act
      testStore.AddStoreBrand(testBrand);
      testStore.Delete();

      List<Store> resultBrandStores = testBrand.GetStores();
      List<Store> testBrandStores = new List<Store> {};

      //Assert
      Assert.Equal(testBrandStores, resultBrandStores);
    }


    [Fact]
    public void AddStoreBrand_AddsBrandToStore()
    {
      //Arrange
      Store testStore = new Store("Shoe Outlet", "shoeoutlet.com");
      testStore.Save();

      Brand testBrand = new Brand ("Adidas", "http://adidas.com/logo.png");
      testBrand.Save();

      Brand testBrand2 = new Brand ("Keen", "http://keen.com");
      testBrand2.Save();

      //Act
      testStore.AddStoreBrand(testBrand);
      testStore.AddStoreBrand(testBrand2);

      List<Brand> result = testStore.GetBrands();
      List<Brand> testList = new List<Brand>{testBrand, testBrand2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetBrands_RetrievesAllBrandsWithStore()
    {
      Store testStore = new Store("How It Be", "itbeshoes.com");
      testStore.Save();

      Brand firstBrand = new Brand("Sketchers", "sketchers.com");
      firstBrand.Save();
      testStore.AddStoreBrand(firstBrand);

      Brand secondBrand = new Brand("Doc Martins", "docmartins.com");
      secondBrand.Save();
      testStore.AddStoreBrand(secondBrand);

      List<Brand> testBrandList = new List<Brand> {firstBrand, secondBrand};
      List<Brand> resultBrandList = testStore.GetBrands();

      Assert.Equal(testBrandList, resultBrandList);
    }

    public void Dispose()
    {
      Brand.DeleteAll();
      Store.DeleteAll();
    }
  }
}
