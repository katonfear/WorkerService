using MobileClient.Class;
using System.Collections.ObjectModel;

namespace MobileClient
{
    public partial class AppShell : Shell
    {
        internal static string Port = string.Empty;
        internal static string Address = string.Empty;
        public AppShell()
        {
            InitializeComponent();
            try
            {
                if (Preferences.Default.ContainsKey("address"))
                {
                    AppShell.Address = Preferences.Default.Get<string>("address", string.Empty);

                }

                if (Preferences.Default.ContainsKey("port"))
                {
                    AppShell.Port = Preferences.Default.Get<string>("port", string.Empty);
                }
            }
            catch(Exception ex) 
            {
                string @out = ex.Message;
            }
        }
    }
}