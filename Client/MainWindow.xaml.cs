using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btShops_Click(object sender, RoutedEventArgs e)
        {
            if (tbServer.Text.Contains(":"))
            {
                try
                {
                    string port = tbServer.Text.Split(':')[1];
                    string ip = tbServer.Text.Split(':')[0];
                    using (TcpClient client = new TcpClient()) 
                    {
                        client.Connect(ip, int.Parse(port));
                        using (var stream = client.GetStream()) 
                        {
                            stream.Write(Encoding.UTF8.GetBytes("shops"));
                            var reader = new StreamReader(stream, Encoding.UTF8);
                            var answer = reader.ReadToEnd();
                            this.textBox.Text = answer;
                        }
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
