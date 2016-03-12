using Nancy;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
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
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        List<Brand> allBrands = Brand.GetAll();
        List<Store> allStores = Store.GetAll();
        models.Add("stores", allStores);
        models.Add("brands", allBrands);
        return View["brands.cshtml", models];
      };

      Get["/stores"] = _ => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        List<Store> allStores = Store.GetAll();
        List<Brand> allBrands = Brand.GetAll();
        models.Add("stores", allStores);
        models.Add("brands", allBrands);
        return View["stores.cshtml", models];
      };

      Get["/brands/new"] = _ => {
        return View["brands_form.cshtml"];
      };

      Post["/brands/new"] = _ => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Brand newBrand = new Brand(Request.Form["brand-name"], Request.Form["brand-logo"]);
        newBrand.Save();
        models.Add("brand", newBrand);
        List<Store> allStores = Store.GetAll();
        List<Brand> allBrands = Brand.GetAll();
        models.Add("stores", allStores);
        models.Add("brands", allBrands);
        return View["brands.cshtml", models];
      };

      Get["/brands/{id}"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Brand newBrand = Brand.Find(parameters.id);
        List<Store> selectedStores = newBrand.GetStores();
        List<Store> allStores = Store.GetAll();
        models.Add("allStores", allStores);
        models.Add("stores", selectedStores);
        models.Add("brand", newBrand);
        return View["brand.cshtml", models];
      };

      Post["/brands/{id}/add_store"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Brand newBrand = Brand.Find(parameters.id);
        Store newStore = Store.Find(Request.Form["select-store"]);
        newBrand.AddStore(newStore);
        List<Store> selectedStores = newBrand.GetStores();
        List<Store> allStores = Store.GetAll();
        models.Add("allStores", allStores);
        models.Add("stores", selectedStores);
        models.Add("brand", newBrand);

        return View["brand.cshtml", models];
      };

      Delete["/brands/{id}"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Brand newBrand = Brand.Find(parameters.id);
        newBrand.Delete();
        List<Brand> allBrands = Brand.GetAll();
        List<Store> allStores = Store.GetAll();
        models.Add("brands", allBrands);
        models.Add("stores", allStores);
        return View["brand.cshtml", models];
      };

      Get["/stores/new"] = _ => {
        return View["stores_form.cshtml"];
      };

      Post["/stores/new"] = _ => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = new Store(Request.Form["store-name"], Request.Form["store-url"]);
        newStore.Save();
        List<Store> allStores = Store.GetAll();
        List<Brand> selectedBrands = newStore.GetBrands();
        List<Brand> allBrands = Brand.GetAll();
        models.Add("stores", allStores);
        models.Add("store", newStore);
        models.Add("brands", allBrands);
        models.Add("brand", selectedBrands);
        return View["stores.cshtml", models];
      };

      Get["/stores/{id}"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = Store.Find(parameters.id);
        List<Brand> selectedBrands = newStore.GetBrands();
        List<Brand> allBrands = Brand.GetAll();
        List<Store> allStores = Store.GetAll();
        models.Add("brands", selectedBrands);
        models.Add("store", newStore);
        models.Add("stores", allStores);
        models.Add("allBrands", allBrands);
        return View["store.cshtml", models];
      };

      Post["/stores/{id}/add_brand"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = Store.Find(parameters.id);
        Brand newBrand = Brand.Find(Request.Form["select-brand"]);
        newStore.AddStoreBrand(newBrand);
        List<Brand> selectedBrands = newStore.GetBrands();
        models.Add("brands", selectedBrands);
        List<Brand> allBrands = Brand.GetAll();
        List<Store> allStores = Store.GetAll();
        models.Add("allBrands", allBrands);
        models.Add("stores", allStores);
        models.Add("store", newStore);
        return View["store.cshtml", models];
      };

      Delete["/stores/{id}/delete"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = Store.Find(parameters.id);
        newStore.Delete();
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

      Delete["/stores/{id}/remove_brands"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = Store.Find(parameters.id);
        List<Brand> selectedBrands = newStore.GetBrands();
        foreach (Brand brand in selectedBrands)
        {
          newStore.RemoveStoreBrand(brand);
        }
        List<Brand> allBrands = Brand.GetAll();
        List<Store> allStores = Store.GetAll();
        models.Add("brands", selectedBrands);
        models.Add("store", newStore);
        models.Add("allbrands", allBrands);
        models.Add("stores", allStores);
        return View["store.cshtml", models];
      };

      Delete["/brands/delete"] = _ => {
        Brand.DeleteAll();
        return View["index.cshtml"];
      };

      Patch["/stores/{id}/update"] = parameters => {
        Dictionary <string, object> models = new Dictionary<string, object>(){};
        Store newStore = Store.Find(parameters.id);
        newStore.Update(Request.Form["store-name"], Request.Form["store-url"]);
        List<Brand> selectedBrands = newStore.GetBrands();
        List<Brand> allBrands = Brand.GetAll();
        models.Add("brands", selectedBrands);
        models.Add("store", newStore);
        models.Add("allBrands", allBrands);
        return View["store.cshtml", models];
      };

    }
  }
}
