using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace StoreBrand
{
  public class Brand
  {
    private int _id;
    private string _name;
    private string _logo;

    public Brand(string Name, string Logo, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _logo = Logo;
    }

    public override bool Equals(System.Object otherBrand)
    {
      if (!(otherBrand is Brand))
      {
        return false;
      }
      else {
        Brand newBrand = (Brand) otherBrand;
        bool IdEquality = this.GetId() == newBrand.GetId();
        bool NameEquality = this.GetName() == newBrand.GetName();
        bool LogoEquality = this.GetLogo() == newBrand.GetLogo();

        return (IdEquality && NameEquality && LogoEquality);
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
    public string GetLogo()
    {
      return _logo;
    }
    public void SetLogo(string newLogo)
    {
      _logo = newLogo;
    }

    public static List<Brand> GetAll()
    {
      List<Brand> AllBrands = new List<Brand>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int BrandId = rdr.GetInt32(0);
        string BrandName = rdr.GetString(1);
        string BrandLogo = rdr.GetString(2);
        Brand NewBrand = new Brand(BrandName, BrandLogo, BrandId);
        AllBrands.Add(NewBrand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllBrands;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO brands (name, logo) OUTPUT INSERTED.id VALUES (@BrandName, @BrandLogo);", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@BrandName";
      nameParam.Value = this.GetName();
      cmd.Parameters.Add(nameParam);

      SqlParameter logoParam = new SqlParameter();
      logoParam.ParameterName = "@BrandLogo";
      logoParam.Value = this.GetLogo();
      cmd.Parameters.Add(logoParam);

      SqlParameter brandIdParam = new SqlParameter();
      brandIdParam.ParameterName = "@BrandId";
      brandIdParam.Value = this.GetId();
      cmd.Parameters.Add(brandIdParam);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM brands;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Brand Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands WHERE id = @BrandId", conn);
      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = id.ToString();
      cmd.Parameters.Add(brandIdParameter);
      rdr = cmd.ExecuteReader();

      int foundBrandId = 0;
      string foundName = null;
      string foundLogo = null;

      while(rdr.Read())
      {
        foundBrandId = rdr.GetInt32(0);
        foundName = rdr.GetString(1);
        foundLogo = rdr.GetString(2);
      }
      Brand foundBrand = new Brand(foundName, foundLogo, foundBrandId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBrand;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM brands WHERE id = @BrandId; DELETE FROM brand_store WHERE brand_id = @BrandId;", conn);
      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();

      cmd.Parameters.Add(brandIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void AddStore(Store newStore)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO brand_store (store_id, brand_id) VALUES (@StoreId, @BrandId)", conn);  //needs more stuff - Store relationship
      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = newStore.GetId();
      cmd.Parameters.Add(StoreIdParameter);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Store> GetStores()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT stores.* FROM brands JOIN brand_store ON (brands.id = brand_store.brand_id) JOIN stores ON (brand_store.store_id = stores.id) WHERE brands.id = @BrandId", conn);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      rdr = cmd.ExecuteReader();

      List<Store> Stores = new List<Store> {};
      int StoreId = 0;
      string StoreName = null;
      string StoreLogo = null;

      while (rdr.Read())
      {
        StoreId = rdr.GetInt32(0);
        StoreName = rdr.GetString(1);
        StoreLogo = rdr.GetString(2);
        Store Store = new Store(StoreName, StoreLogo, StoreId);
        Stores.Add(Store);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      return Stores;
    }
  }
}
