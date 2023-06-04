using LiteDB;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Class
{
    internal class DbControler : IDisposable
    {
        public string GetFilePathToShops() 
        {
            if (!Directory.Exists(FileSystem.Current.AppDataDirectory)) 
            {
                Directory.CreateDirectory(FileSystem.Current.AppDataDirectory);
            }
            return System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "shop.db");
        }

        public void SaveShops(List<Shop> list) 
        {
            using (var db = new LiteDatabase(GetFilePathToShops())) 
            {
                db.DropCollection("shops");
                var col = db.GetCollection<Shop>("shops");
                foreach (var shop in list) 
                {
                    col.Insert(shop);
                }
            }
        }

        public ObservableCollection<Shop> GetShops() 
        {
            using (var db = new LiteDatabase(GetFilePathToShops()))
            {
                if (db.CollectionExists("shops"))
                {
                    var collection = new ObservableCollection<Shop>();
                    var col = db.GetCollection<Shop>("shops");
                    foreach (var element in col.FindAll()) 
                    { 
                        collection.Add(element);
                    }
                    return collection;
                }
                else 
                {
                    return new ObservableCollection<Shop>();
                }
            }
        }

        public string GetFilePathToProducts()
        {
            if (!Directory.Exists(FileSystem.Current.AppDataDirectory))
            {
                Directory.CreateDirectory(FileSystem.Current.AppDataDirectory);
            }
            return System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "product.db");
        }

        public void SaveProducts(List<Product> list)
        {
            using (var db = new LiteDatabase(GetFilePathToProducts()))
            {
                db.DropCollection("products");
                var col = db.GetCollection<Product>("products");
                foreach (var product in list)
                {
                    col.Insert(product);
                }
            }
        }

        public ObservableCollection<Product> GetProducts()
        {
            using (var db = new LiteDatabase(GetFilePathToProducts()))
            {
                if (db.CollectionExists("products"))
                {
                    var collection = new ObservableCollection<Product>();
                    var col = db.GetCollection<Product>("products");
                    foreach (var element in col.FindAll())
                    {
                        collection.Add(element);
                    }
                    return collection;
                }
                else
                {
                    return new ObservableCollection<Product>();
                }
            }
        }

        public void SaveCart(List<Product> list)
        {
            using (var db = new LiteDatabase(GetFilePathToProducts()))
            {
                db.DropCollection("cart");
                var col = db.GetCollection<Product>("cart");
                foreach (var product in list)
                {
                    col.Insert(product);
                }
            }
        }

        public ObservableCollection<Product> GetCart()
        {
            using (var db = new LiteDatabase(GetFilePathToProducts()))
            {
                if (db.CollectionExists("cart"))
                {
                    var collection = new ObservableCollection<Product>();
                    var col = db.GetCollection<Product>("cart");
                    foreach (var element in col.FindAll())
                    {
                        collection.Add(element);
                    }
                    return collection;
                }
                else
                {
                    return new ObservableCollection<Product>();
                }
            }
        }

        public void SavePurchased(List<Product> list)
        {
            using (var db = new LiteDatabase(GetFilePathToProducts()))
            {
                db.DropCollection("purchased");
                var col = db.GetCollection<Product>("purchased");
                foreach (var product in list)
                {
                    col.Insert(product);
                }
            }
        }

        public ObservableCollection<Product> GetPurchased()
        {
            using (var db = new LiteDatabase(GetFilePathToProducts()))
            {
                if (db.CollectionExists("purchased"))
                {
                    var collection = new ObservableCollection<Product>();
                    var col = db.GetCollection<Product>("purchased");
                    foreach (var element in col.FindAll())
                    {
                        collection.Add(element);
                    }
                    return collection;
                }
                else
                {
                    return new ObservableCollection<Product>();
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
