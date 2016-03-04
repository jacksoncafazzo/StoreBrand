using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace StoreBrand
{
  public class Store
  {
    private int _id;
    private string _name;
    private string _url;

    public Store(string Name, string Url, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _url = Url;
    }

    public override bool Equals(System.Object otherStore)
    {
      if (!(otherStore is Store))
      {
        return false;
      }
      else
      {
        Store newStore = (Store) otherStore;
        bool idEquality = this.GetId() == newStore.GetId();
        bool nameEquality = this.GetName() == newStore.GetName();
        bool UrlEquality = this.GetUrl() == newStore.GetUrl();
        return (idEquality && nameEquality && UrlEquality);
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
    public string GetUrl()
    {
      return _url;
    }
    public void SetUrl(string newNumber)
    {
      _url = newNumber;
    }

    public static List<Store> GetAll()
    {
      List<Store> allStores = new List<Store>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int storeId = rdr.GetInt32(0);
        string storeName = rdr.GetString(1);
        string Url = rdr.GetString(2);
        Store newStore = new Store(storeName, Url, storeId);
        allStores.Add(newStore);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allStores;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stores (name, url) OUTPUT INSERTED.id VALUES (@StoreName, @Url); INSERT INTO store_brand (store_id) OUTPUT INSERTED.id VALUES (@StoreId)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@StoreName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      SqlParameter UrlParameter = new SqlParameter();
      UrlParameter.ParameterName = "@Url";
      UrlParameter.Value = this.GetUrl();
      cmd.Parameters.Add(UrlParameter);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM stores;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Store Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores WHERE id = @StoreID;", conn);
      SqlParameter StoreIDParemeter = new SqlParameter();
      StoreIDParemeter.ParameterName = "@StoreId";
      StoreIDParemeter.Value = id.ToString();
      cmd.Parameters.Add(StoreIDParemeter);
      rdr = cmd.ExecuteReader();

      int foundStoreId = 0;
      string foundStoreName = null;
      string foundUrl = null;

      while(rdr.Read())
      {
        foundStoreId = rdr.GetInt32(0);
        foundStoreName = rdr.GetString(1);
        foundUrl = rdr.GetString(2);
      }
      Store foundStore = new Store(foundStoreName, foundUrl, foundStoreId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStore;
    }

    public void AddStoreBrand(Brand newBrand)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO store_brand (store_id, brand_id) VALUES (@StoreId, @BrandId)", conn);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = newBrand.GetId();
      cmd.Parameters.Add(brandIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Brand> GetBrands()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT brands.* FROM stores JOIN store_brand ON (stores.id = store_brand.store_id) JOIN brands ON (store_brand.brand_id = brands.id) WHERE stores.id = @StoreId", conn);
      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

      rdr = cmd.ExecuteReader();

      List<Brand> brands = new List<Brand> {};
      int brandId = 0;
      string BrandName = null;
      string BrandLogo = null;

      while(rdr.Read())
      {
        brandId = rdr.GetInt32(0);
        BrandName = rdr.GetString(1);
        BrandLogo = rdr.GetString(2);
        Brand brand = new Brand(BrandName, BrandLogo, brandId);
        brands.Add(brand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return brands;
    }

    public void Delete()
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("DELETE FROM stores WHERE id = @StoreId; DELETE FROM store_brand WHERE store_id = @StoreId;", conn);
       SqlParameter storeIdParameter = new SqlParameter();
       storeIdParameter.ParameterName = "@StoreId";
       storeIdParameter.Value = this.GetId();

       cmd.Parameters.Add(storeIdParameter);
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
