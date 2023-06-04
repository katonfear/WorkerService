
namespace MobileClient
{
    public partial class Settings : ContentPage
    {
        string Adress { get; set; } = AppShell.Address;
        string Port { get; set; } = AppShell.Port;
        public Settings()
        {
            InitializeComponent();
            addres.Text = Adress;
            port.Text = Port;
        }

        private void save_Clicked(object sender, EventArgs e)
        {
            try 
            {
                AppShell.Address = addres.Text;
                AppShell.Port = port.Text;
                Adress = AppShell.Address;
                Port = AppShell.Port;
                Preferences.Default.Set<string>("address", Adress);
                Preferences.Default.Set<string>("port", Port);
                DisplayAlert("Success", "Dane zostały zapisane.", "OK");
            }
            catch(Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}