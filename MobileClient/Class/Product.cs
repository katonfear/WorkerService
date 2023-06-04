using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Class
{
    public class Product
    {
        [JsonIgnore]
        internal static ObservableCollection<Product> Products = new ObservableCollection<Product>();
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Shop { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        [JsonIgnore]
        public string ShowNameCart { get { return $"{Name} w Sklepie {Shop}."; } }
        [JsonIgnore]
        public string ShowName { get { return $"{Name} - w Sklepie {Shop} za {Price} zł"; } }
        [JsonIgnore]
        public string ShowNameLong { get { return $"{Name} - w Sklepie {Shop} za {Price} zł w ilości {Qty}"; } }
        public double Qty { get; set; } = 0;

        [JsonIgnore]
        public bool IsPurchased { get; set; } = false;

        public Product() { }

        public Product(string name, string barcode, string shop, double price, int qty)
        {
            Name = name;
            Barcode = barcode;
            Shop = shop;
            Price = price;
            Qty = qty;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Product FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Product>(json);
        }

        public static ObservableCollection<Product> FromJsonList(string json)
        {
            return JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
        }

        public static string ListToJson(ObservableCollection<Product> list)
        {
            return JsonConvert.SerializeObject(list);
        }
    }
}
