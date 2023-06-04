using MobileClient.Class;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MobileClient
{
    public partial class Products : ContentPage
    {
        public Products()
        {
            InitializeComponent();
        }

        private void CounterBtn_Clicked(object sender, EventArgs e)
        {
            sendProducts();
            try
            {
                string port = AppShell.Port;
                string ip = AppShell.Address;


                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ip, int.Parse(port));
                    using (var stream = client.GetStream())
                    {
                        stream.Write(Encoding.UTF8.GetBytes("products"));
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        var answer = reader.ReadToEnd();
                        var List = MobileClient.Class.Product.FromJsonList(answer);
                        MobileClient.Class.Product.Products.Clear();
                        foreach (var element in List)
                        {
                            MobileClient.Class.Product.Products.Add(element);
                        }
                        new DbControler().SaveProducts(MobileClient.Class.Product.Products.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void sendProducts() 
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
                        string data = $"addpord{split}{Class.Product.ListToJson(MobileClient.Class.Product.Products)}";
                        stream.Write(Encoding.UTF8.GetBytes(data));
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        var answer = reader.ReadToEnd();
                        var msg = MobileClient.Class.Answer.FromJson(answer);
                        if (msg != null && msg.Message != "all products was added")
                            DisplayAlert("Alert", msg.Message, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void AddBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var shell = (AppShell.Current as AppShell);
                foreach (var item in shell.Items)
                {
                    if (item.Title == "Dodaj produkt")
                    {
                        shell.CurrentItem = item;
                        break;
                    }
                }
            }
            catch(Exception ex) 
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}