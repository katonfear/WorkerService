using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zoft.MauiExtensions.Core.Extensions;
using zoft.MauiExtensions.Core.ViewModels;

namespace MobileClient.Models
{
    internal partial class ProductItem : ObservableObject
    {
        [ObservableProperty]
        public string _name;

        [ObservableProperty]
        public string _barcode;

        [ObservableProperty]
        public string _shop;

        public Class.Product ToProduct() 
        {
            return new Class.Product() { Name = Name, Barcode = Barcode, Shop = Shop };
        }
    }

    internal partial class ProductItemViewModel : CoreViewModel
    {

        [ObservableProperty]
        private ObservableCollection<ProductItem> _filteredList;

        [ObservableProperty]
        private ProductItem _selectedItem;

        internal static ObservableCollection<ProductItem> GetProductItems() 
        {
            List<ProductItem> @return= new List<ProductItem>();
            foreach (var prod in Class.Product.Products) 
            {
                @return.Add(new ProductItem() { Barcode = prod.Barcode, Name = prod.Name, Shop = prod.Shop });
            }
            return new(@return);
        }

        public Command<string> TextChangedCommand { get; }

        public ProductItemViewModel() 
        {
            this.FilteredList = new ObservableCollection<ProductItem>();
            this.SelectedItem = null;
            TextChangedCommand = new Command<string>(FilterList);
        }

        internal void FilterList(string filter)
        {
            SelectedItem = null;
            FilteredList.Clear();

            FilteredList.AddRange(GetProductItems().Where(t => (t.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(filter)) ||
                                                   (t.Shop.Contains(filter, StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(filter))));
        }

    }
}
