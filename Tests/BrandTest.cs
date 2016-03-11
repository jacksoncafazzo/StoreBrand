using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StoreBrand
{
  public class BrandTest : IDisposable
  {
    public BrandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoestores_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void DatabaseEmptyAtFirst()
    {
      //Arange, Act
      int result = Brand.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void EqualOverrideTrueForSameName()
    {
      //Arrange, Act
      Brand firstBrand = new Brand("Bob Marley", "bobmarley.com");
      Brand secondBrand = new Brand("Bob Marley", "bobmarley.com");

      //Assert
      Assert.Equal(firstBrand, secondBrand);
    }

    [Fact]
    public void Save()
    {
      //Arrange
      Brand testBrand = new Brand("Converse", "converse.com/logo");
      testBrand.Save();

      //Act
      List<Brand> result = Brand.GetAll();
      List<Brand> testList = new List<Brand>{testBrand};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void SaveAssignsIdToObject()
    {
      //Arrange
      Brand testBrand = new Brand("Nike", "nike.com");
      testBrand.Save();

      //Act
      Brand savedBrand = Brand.GetAll()[0];

      int result = savedBrand.GetId();
      int testId = testBrand.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void FindFindsBrandInDatabase()
    {
      //Arrange
      Brand testBrand = new Brand("Hov", "hov.com");
      testBrand.Save();

      //Act
      Brand result = Brand.Find(testBrand.GetId());

      //Assert
      Assert.Equal(testBrand, result);
    }

    [Fact]
    public void AddStore_AddsStoreToBrand()
    {
      //Arrange
      Store testStore = new Store("Tight Laces", "tightlaces.com");
      testStore.Save();

      Brand testBrand = new Brand("Montana S##t Kickers", "bigboots.com");
      testBrand.Save();

      //Act
      testBrand.AddStore(testStore);

      List<Store> result = testBrand.GetStores();
      List<Store> testList = new List<Store>{testStore};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetStores_ReturnsAllBrandStores()
    {
      //Arrange
      Brand testBrand = new Brand("Generic", "N/A");
      testBrand.Save();

      Store testStore1 = new Store("Dorian's Shoe Store", "none");
      testStore1.Save();

      Store testStore2 = new Store("Nob Hill Cobbler", "none");
      testStore2.Save();

      //Act
      testBrand.AddStore(testStore1);
      testBrand.AddStore(testStore2);
      List<Store> result = testBrand.GetStores();
      List<Store> testList = new List<Store> {testStore1, testStore2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesBrandshoestoressFromDatabase()
    {
      //Arrange
      Store testStore = new Store("Get Shoes Here", "gotshoe.com");
      testStore.Save();

      Brand testBrand = new Brand("Puma", "puma.com");
      testBrand.Save();

      //Act
      testBrand.AddStore(testStore);
      testBrand.Delete();

      List<Brand> resultStoreBrands = testStore.GetBrands();
      List<Brand> testStoreBrands = new List<Brand> {};

      //Assert
      Assert.Equal(testStoreBrands, resultStoreBrands);
    }

    public void Dispose()
    {
      Brand.DeleteAll();
      Store.DeleteAll();
    }
  }
}
