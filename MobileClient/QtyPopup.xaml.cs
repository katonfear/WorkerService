using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Views;
using MobileClient.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace MobileClient;

public partial class QtyPopup : Popup
{
    public class PriceQtyEventArgs : EventArgs
    {
        public Class.Product Product { get; set; } = null;
    }

    public delegate void PriceQtyEventHandler(object sender, PriceQtyEventArgs e);

    public event PriceQtyEventHandler PriceQty;

    private Class.Product product;

    public QtyPopup(Class.Product product)
	{
		InitializeComponent();
        this.product = product;
        this.BindingContext = new PriceAndQty();
    }

    private void Set_Clicked(object sender, EventArgs e)
    {
        var binding = (this.BindingContext as PriceAndQty);
        if (binding != null)
        {
            if (binding.Price.HasValue && binding.Qty.HasValue)
            {
                product.Price = binding.Price.Value;
                product.Qty = binding.Qty.Value;
                PriceQty?.Invoke(this, new PriceQtyEventArgs() { Product = product });
                this.Close(true);
            }
            else 
            {
                QtyPopup.showMessage("Proszê podaæ iloœæ oraz cenê");
            }
        }
    }

    public static void showMessage(string msg) 
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;

        var toast = Toast.Make(msg, duration, fontSize);

        toast.Show(cancellationTokenSource.Token);
    }

    private void close_Clicked(object sender, EventArgs e)
    {
        this.Close(false);
    }
}