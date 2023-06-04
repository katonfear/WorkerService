using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Models
{
    internal class PriceAndQty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double? _price;
        private double? _qty;

        public double? Price { get { return _price; } set { _price = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price")); } }
        public double? Qty { get { return _qty; } set { _qty = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Qty")); } }
    }
}
