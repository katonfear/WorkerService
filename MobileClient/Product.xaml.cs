using CommunityToolkit.Maui.Alerts;
using MobileClient.Class;
using MobileClient.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MobileClient
{
    public partial class Product : ContentPage
    {
        public ProductModel Model { get; set; }
        public Product()
        {
            InitializeComponent();
            this.SizeChanged += Product_SizeChanged;
            Model= new ProductModel();
            this.BindingContext= Model;
        }

        private void Product_SizeChanged(object sender, EventArgs e)
        {
            this.pick.WidthRequest = this.Width - 10;
        }

        private void save_Clicked(object sender, EventArgs e)
        {
            try 
            {
                if (!string.IsNullOrEmpty(Model.Name) && !string.IsNullOrEmpty(Model.Barcode))
                {
                    string port = AppShell.Port;
                    string ip = AppShell.Address;
                    char split = (char)2;
                    using (TcpClient client = new TcpClient())
                    {
                        client.Connect(ip, int.Parse(port));
                        using (var stream = client.GetStream())
                        {
                            var prod = Model.ChangeToProduct();
                            var list = new ObservableCollection<Class.Product>();
                            list.Add(prod);
                            string data = $"addpord{split}{Class.Product.ListToJson(list)}";
                            stream.Write(Encoding.UTF8.GetBytes(data));
                            var reader = new StreamReader(stream, Encoding.UTF8);
                            var answer = reader.ReadToEnd();
                            var msg = MobileClient.Class.Answer.FromJson(answer);
                            if (msg != null && msg.Message != "all products was added")
                                DisplayAlert("Alert", msg.Message, "OK");
                            else
                                foreach (var prod1 in list)
                                {
                                    if (!Class.Product.Products.Any(p => p.Name == prod1.Name && p.Barcode == prod1.Barcode))
                                    {
                                        Class.Product.Products.Add(prod1);
                                    }
                                }
                            using (DbControler conn = new DbControler())
                            {
                                conn.SaveProducts(Class.Product.Products.ToList());
                            }
                            if (msg != null && msg.Message == "all products was added")
                            {
                                QtyPopup.showMessage($"Dodano produkt {Model.Name}");
                                Model.Barcode = "";
                                Model.Name = String.Empty;
                                Model.Price = 0;
                                Model.Qty = 0;
                                Model.Shop = "Wszystkie";
                            }
                        }
                    }
                }
                else 
                {
                    QtyPopup.showMessage($"Brak nazwy produktu");
                }
            }
            catch(Exception ex) 
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}