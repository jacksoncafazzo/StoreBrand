using Nancy;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace StoreBrand
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

      Get["/brands"] = _ => {
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Post["/brands"] = _ => {
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Get["/stores"] = _ => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        List<Store> AllStores = Store.GetAll();
        List<Brand> allBrands = Brand.GetAll();
        models.Add("stores", AllStores);
        models.Add("brands", allBrands);
        return View["stores.cshtml", models];
      };

      Get["/brands/new"] = _ => {
        return View["brands_form.cshtml"];
      };

      Post["/brands/new"] = _ => {
        Brand newBrand = new Brand(Request.Form["brand-name"], Request.Form["brand-logo"]);
        newBrand.Save();
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Get["/brands/{id}"] = parameters => {
        Brand newBrand = Brand.Find(parameters.id);
        return View["brand.cshtml", newBrand];
      };

      Delete["/brands/{id}"] = parameters => {
        Brand newBrand = Brand.Find(parameters.id);
        newBrand.Delete();
        List<Brand> AllBrands = Brand.GetAll();
        return View["brand.cshtml", AllBrands];
      };

      Get["/stores/new"] = _ => {
        return View["stores_form.cshtml"];
      };

      Post["/stores/new"] = _ => {
        Store newStore = new Store(Request.Form["store-name"], Request.Form["store-url"]);
        newStore.Save();
        // Brand newBrand = new Brand(Request.Form["brand-name"], Request.Form["brand-logo"]);
        // newBrand.Save();
        // newStore.AddStoreBrand(newBrand);
        List<Store> AllStores = Store.GetAll();
        return View["stores.cshtml", AllStores];
      };

      Get["/stores/{id}"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = Store.Find(parameters.id);
        List<Brand> selectedBrands = newStore.GetBrands();
        List<Brand> allBrands = Brand.GetAll();
        models.Add("brands", selectedBrands);
        models.Add("store", newStore);
        models.Add("allbrands", allBrands);
        return View["store.cshtml", models];
      };

      Post["/stores/{id}/add_brand"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = Store.Find(parameters.id);
        Brand newBrand = Brand.Find(Request.Form["select-brand"]);
        newStore.AddStoreBrand(newBrand);
        List<Brand> allBrands = Brand.GetAll();
        List<Store> allStores = Store.GetAll();
        models.Add("brands", allBrands);
        models.Add("stores", allStores);
        return View["stores.cshtml", models];
      };

      Delete["/stores/delete"] = _ => {
        Store.DeleteAll();
        return View["index.cshtml"];
      };

      Delete["/brands/delete"] = _ => {
        Brand.DeleteAll();
        return View["index.cshtml"];
      };


    }
  }
}
