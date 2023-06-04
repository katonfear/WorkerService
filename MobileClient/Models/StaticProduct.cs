using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Models
{
    public class ProductModel: INotifyPropertyChanged
    {
        private string name = string.Empty;
        private string barcode = string.Empty;
        private string shop = "Wszystkie";
        private double price = -1;
        private int qty = -1;

        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get { return name; } set { name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); } }
        public string Barcode { get { return barcode; } set { barcode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Barcode")); } }
        public string Shop { get { return shop; } set { shop = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Shop")); } }
        public double Price { get { return price; } set { price = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price")); } }
        public int Qty { get { return qty; } set { qty = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Qty")); } }

        public ProductModel() { }

        public Class.Product ChangeToProduct() 
        {
            return new Class.Product(this.name, this.barcode, this.shop, this.price, this.qty);
        }
    }
}
