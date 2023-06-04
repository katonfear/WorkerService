using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Class
{
    public class CartListModel: INotifyPropertyChanged
    {
        public ObservableCollection<Product> ProductsToPurchase { get; set; } = new ObservableCollection<Product>();

        public ObservableCollection<Product> ProductsPurchased { get; set; } = new ObservableCollection<Product>();

        public CartListModel()  { }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
