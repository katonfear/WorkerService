using MobileClient.Class;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MobileClient
{
    public partial class Shops : ContentPage
    {
        public Shops()
        {
            InitializeComponent();
            try
            {
                var list = new DbControler().GetShops();
                Shop.Shops.Clear();
                foreach (var shop in list) 
                {
                    Shop.Shops.Add(shop);
                }
            }
            catch (Exception ex) 
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }

            try
            {
                var list = new DbControler().GetProducts();
                Class.Product.Products.Clear();
                foreach (var prod in list)
                {
                    Class.Product.Products.Add(prod);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                string port = AppShell.Port;
                string ip = AppShell.Address;
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ip, int.Parse(port));
                    using (var stream = client.GetStream())
                    {
                        stream.Write(Encoding.UTF8.GetBytes("shops"));
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        var answer = reader.ReadToEnd();
                        var List = Shop.FromJsonList(answer);
                        Shop.Shops.Clear();
                        foreach (var element in List) 
                        {
                            Shop.Shops.Add(element);
                        }
                        new DbControler().SaveShops(Shop.Shops.ToList());
                    }
                }
            }
            catch (Exception ex) 
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}