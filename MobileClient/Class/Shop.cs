using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Class
{
    public class Shop
    {
        [JsonIgnore]
        internal static ObservableCollection<Shop> Shops = new ObservableCollection<Shop>();
        [JsonIgnore]
        internal static List<string> ListShops { get { return GetShopNames(); } }

        [JsonProperty("shopName", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; } = int.MinValue;

        [JsonIgnore]
        public string ShowName { get { return $"{this.Id} - {this.Name}"; } }

        public Shop() 
        {
        }

        public Shop(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Shop FromJson(string json) 
        {
            return JsonConvert.DeserializeObject<Shop>(json);
        }

        public static ObservableCollection<Shop> FromJsonList(string json) 
        {
            return JsonConvert.DeserializeObject<ObservableCollection<Shop>>(json);
        }

        public static string ListToJson(ObservableCollection<Shop> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        public static List<string> GetShopNames() 
        {
            var list = Shops.Select(shop => shop.Name).ToList();
            list.Insert(0,"Wszystkie");
            return list;
        }
    }
}
