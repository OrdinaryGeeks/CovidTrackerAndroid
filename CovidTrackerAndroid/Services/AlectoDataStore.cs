using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using CovidTrackerAndroid.Models;
using System.Net;
using Xamarin.Android.Net;
using Android.Provider;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CovidTrackerAndroid.Services
{
    public class AlectoDataStore
    {
        HttpClient client;
       // IEnumerable<Item> items;
   
        IEnumerable<Association> associations;
        IEnumerable<LatLongGroup> latLongGroups;
        IEnumerable<TimeBlock> timeBlocks;
        IEnumerable<User> users;


        public AlectoDataStore()
        {
            client = new HttpClient(new AndroidClientHandler());
            client.BaseAddress = new Uri("http://www.ordinarygeeks.com/OrdinaryGeeks/");

            users = new List<User>();
            timeBlocks = new List<TimeBlock>();
            latLongGroups = new List<LatLongGroup>();
            associations = new List<Association>();
        }

        bool IsConnected => Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet;


        /*
        public async Task<IEnumerable<Category>> GetCategoriesAsync(bool forceRefresh = false)
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/APICategories");
                categories = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Category>>(json));
            }

            return categories;
        }

        public async Task<IEnumerable<ShoppingLocation>> GetLocationsAsync(bool forceRefresh = false)
        {
            if ( IsConnected)
            {
                var json = await client.GetStringAsync($"api/Locations");
                locations = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<ShoppingLocation>>(json));
            }

            return locations;
        }

        public  ShoppingLocation GetLocationByName(string locName)
        {
            ShoppingLocation loc = new ShoppingLocation();
            if (IsConnected)
            {
                var json =  client.GetStringAsync($"api/Locations/LocationName/" + locName);
                 loc =   JsonConvert.DeserializeObject<ShoppingLocation>(json.Result);

                return loc;

            }
            return loc;
        }
        public async Task<Product> GetProductByName(string prodName)
        {
            Product prod = new Product();

          
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/ApiProducts/ProductName/" + prodName);
                prod = await Task.Run(() => JsonConvert.DeserializeObject<Product>(json));

                return prod;

            }
            return prod;
        }
        public async Task<IEnumerable<Category>> GetCategoriesByLocationIdAsync(int locId, bool forceRefresh = false)
            {
           // IEnumerable<Category> cats;
                if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/ApiCategories/LocationID/"+locId);
       var cats = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Category>>(json));
                return cats;
            }
            return null;
            }

        public async Task<bool> PutProductAsync(Product product)
        {
            if (product == null || !IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(product);

            var response = await client.PutAsync($"api/APIProducts/"+product.ApiProductID, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PostProductAsync(Product product)
        {
            if (product == null || !IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(product);

            var response = await client.PostAsync($"api/APIProducts/", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdLocationIdAsync(int catId, int locId)
        {
            if (IsConnected)
            {
                var json = await client.GetAsync($"api/APISubcategories/CategoryID/" + catId + "/LocationID/"+locId);

                var json2 = await json.Content.ReadAsStringAsync();
                subcategories = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Subcategory>>(json2));
            }

            return subcategories;
        }
        public async Task<IEnumerable<Product>> GetProductsBySubcategoryIdLocationIdAsync(int subcatId, int locId)
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/APIProducts/SubcategoryID/" + subcatId + "/LocationID/"+locId);
                products = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Product>>(json));
            }

            return products;
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int catId)
        {
            if (IsConnected)
            {
                var json = await client.GetAsync($"api/APISubcategories/CategoryID/"+catId);

                var json2 = await json.Content.ReadAsStringAsync();
                subcategories = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Subcategory>>(json2));
            }

            return subcategories;
        }
        public async Task<IEnumerable<Product>> GetProductsBySubcategoryIdAsync(int subcatId)
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/APIProducts/SubcategoryID/" + subcatId);
                products = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Product>>(json));
            }

            return products;
        }

        public async Task<IEnumerable<Subcategory>> GetSubcategoriesAsync(bool forceRefresh = false)
        {
            if (IsConnected)
            {
                var json = await client.GetAsync($"api/APISubcategories");

                var json2=await json.Content.ReadAsStringAsync();
                subcategories = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Subcategory>>(json2));
            }

            return subcategories;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync(bool forceRefresh = false)
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/APIProducts");
                products = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Product>>(json));
            }

            return products;
        }

        public async Task<ShoppingLocation> GetSingleShoppingLocationAsync(int locID, bool forceRefresh = false)
        {

            if(IsConnected)
            {
                var json = await client.GetStringAsync($"api/Locations/" + locID);
                return await Task.Run(() => JsonConvert.DeserializeObject<ShoppingLocation>(json));
            }

            return null;

        }
        

        public async Task<bool> AddSubcategoryAsync(Subcategory subcategory)
        {
            if (subcategory == null || !IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(subcategory);

            var response = await client.PostAsync($"api/APISubcategories", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSubcategoryAsync(Subcategory subcategory)
        {
            if (subcategory == null &&
                !IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(subcategory);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

           // var response = await client.GetStringAsync(new Uri($"api/ApiSubcategories/1"));
            var response = await client.PutAsync(($"api/ApiSubcategories/{subcategory.ApiSubcategoryID }"), new StringContent(serializedItem, Encoding.UTF8, "Application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSubcategoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/APISubcategories/{id}");

            return response.IsSuccessStatusCode;
        }

        */

       
    }
}
