using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using MobileClient.Class;
using MobileClient.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MobileClient
{
    public partial class Cart : ContentPage
    {
        private Class.Product selected = null;
        private Class.CartListModel cartListModel = new CartListModel();
        public Cart()
        {
            InitializeComponent();
            selProd.BindingContext = new ProductItemViewModel();
            SelectedItems.SetBinding(CollectionView.ItemsSourceProperty, "ProductsToPurchase");
            SelectedItems.BindingContext = cartListModel;
            PurchasedItems.SetBinding(CollectionView.ItemsSourceProperty, "ProductsPurchased");
            PurchasedItems.BindingContext = cartListModel;
            try
            {
                using (DbControler db = new DbControler())
                {
                    var list = db.GetCart();
                    cartListModel.ProductsToPurchase.Clear();
                    foreach (var prod in list)
                    {
                        cartListModel.ProductsToPurchase.Add(prod);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }

            try
            {
                using (DbControler db = new DbControler())
                {
                    var list = db.GetPurchased();
                    cartListModel.ProductsPurchased.Clear();
                    foreach (var prod in list)
                    {
                        cartListModel.ProductsPurchased.Add(prod);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void selProd_SuggestionChosen(object sender, zoft.MauiExtensions.Controls.AutoCompleteEntrySuggestionChosenEventArgs e)
        {
            try
            {
                (selProd.BindingContext as ProductItemViewModel).SelectedItem = e.SelectedItem as ProductItem;
                selected = (e.SelectedItem as ProductItem).ToProduct();
                (SelectedItems.BindingContext as Class.CartListModel).ProductsToPurchase.Add(selected);
                selProd.Text = string.Empty;
            }
            catch (Exception ex) 
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            try
            {
                var val = (sender as Button).BindingContext as Class.Product;
                (SelectedItems.BindingContext as Class.CartListModel).ProductsToPurchase.Remove(val);
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var shell = (AppShell.Current as AppShell);
                foreach (var item in shell.Items)
                {
                    if (item.Title == "Skanuj")
                    {
                        shell.CurrentItem = item;
                        WeakReferenceMessenger.Default.Register<BarCodeMessage>(this, (r, m) =>
                        {
                            string message = m.Value;
                            var first = cartListModel.ProductsToPurchase.FirstOrDefault(a=>a.Barcode == message);
                            if (first != null) 
                            {
                                SetQtyPrice(first);
                            }
                            WeakReferenceMessenger.Default.UnregisterAll(this);
                        });
                        break;
                    }
                }
            }
            catch (Exception ex) 
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void save_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (DbControler conn = new DbControler())
                {
                    conn.SaveCart(cartListModel.ProductsToPurchase.ToList());
                }
                QtyPopup.showMessage("Dane zostały zapisane.");
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void Purchased_Clicked(object sender, EventArgs e)
        {
            try
            {
                var val = (sender as Button).BindingContext as Class.Product;
                SetQtyPrice(val);
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void savePurchased_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (DbControler conn = new DbControler())
                {
                    conn.SavePurchased(cartListModel.ProductsPurchased.ToList());
                    conn.SaveCart(cartListModel.ProductsToPurchase.ToList());
                }
                QtyPopup.showMessage("Dane zostały zapisane.");
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void sendToServer_Clicked(object sender, EventArgs e)
        {
            string msg = string.Empty;
            sendProducts(ref msg);
        }

        private void sendProducts(ref string @out)
        {
            try
            {
                string port = AppShell.Port;
                string ip = AppShell.Address;
                char split = (char)2;
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ip, int.Parse(port));
                    using (var stream = client.GetStream())
                    {
                        string data = $"addpord{split}{Class.Product.ListToJson(cartListModel.ProductsPurchased)}";
                        stream.Write(Encoding.UTF8.GetBytes(data));
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        var answer = reader.ReadToEnd();
                        var msg = MobileClient.Class.Answer.FromJson(answer);
                        if (msg != null && msg.Message != "all products was added")
                            DisplayAlert("Alert", msg.Message, "OK");
                        else
                        {
                            cartListModel.ProductsPurchased.Clear();
                            cartListModel.ProductsToPurchase.Clear();
                            QtyPopup.showMessage("Dane zostały zapisane.");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                @out = ex.Message;
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void DeletePD_Clicked(object sender, EventArgs e)
        {
            try
            {
                var val = (sender as Button).BindingContext as Class.Product;
                (PurchasedItems.BindingContext as Class.CartListModel).ProductsPurchased.Remove(val);
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void ClearAll_Clicked(object sender, EventArgs e)
        {
            try
            {
                cartListModel.ProductsPurchased.Clear();
                cartListModel.ProductsToPurchase.Clear();
                using (DbControler conn = new DbControler())
                {
                    conn.SavePurchased(cartListModel.ProductsPurchased.ToList());
                    conn.SaveCart(cartListModel.ProductsToPurchase.ToList());
                }
                QtyPopup.showMessage("Dane zostały usunięte.");
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void SetQtyPrice(Class.Product product) 
        {
            try
            {
                var popup = new QtyPopup(product);
                popup.Size = new Size() { Height = 300, Width = 300 };
                popup.PriceQty += Popup_PriceQty;
                this.ShowPopup(popup);
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void Popup_PriceQty(object sender, QtyPopup.PriceQtyEventArgs e)
        {
            try
            {
                (SelectedItems.BindingContext as Class.CartListModel).ProductsToPurchase.Remove(e.Product);
                (PurchasedItems.BindingContext as Class.CartListModel).ProductsPurchased.Add(e.Product);
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}