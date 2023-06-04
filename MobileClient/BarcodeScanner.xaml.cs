using CommunityToolkit.Mvvm.Messaging;
using MobileClient.Models;
using System.Diagnostics;

namespace MobileClient;

public partial class BarcodeScanner : ContentPage
{
    public string ScanedBarcode { get; set; } = string.Empty;
    public BarcodeScanner()
	{

        try
        {
            InitializeComponent();
            cameraView.BarcodeDetected += CameraView_BarcodeDetected;
            cameraView.BarCodeOptions = new Camera.MAUI.ZXingHelper.BarcodeDecodeOptions
            {
                AutoRotate = true,
                PossibleFormats = { ZXing.BarcodeFormat.QR_CODE, ZXing.BarcodeFormat.EAN_13, ZXing.BarcodeFormat.EAN_8, ZXing.BarcodeFormat.CODE_128, ZXing.BarcodeFormat.CODE_39, ZXing.BarcodeFormat.CODE_93, ZXing.BarcodeFormat.DATA_MATRIX, ZXing.BarcodeFormat.MAXICODE },
                ReadMultipleCodes = false,
                TryHarder = true,
                TryInverted = true
            };
            cameraView.BarCodeDetectionFrameRate = 10;
            cameraView.BarCodeDetectionMaxThreads = 5;
            cameraView.ControlBarcodeResultDuplicate = true;
            cameraView.BarCodeDetectionEnabled = true;
            this.NavigatedTo += BarcodeScanner_NavigatedTo;
        }
        catch (Exception ex) 
        {
            DisplayAlert("Alert", ex.Message, "OK");
        }
    }

    private void BarcodeScanner_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        try
        {
            cameraView.Camera = cameraView.Cameras.FirstOrDefault();
            if (cameraView.Camera != null)
            {
                MainThread.BeginInvokeOnMainThread(new Action(async () =>
                {
                    try
                    {
                        await cameraView.StopCameraAsync();
                        await cameraView.StartCameraAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Alert", ex.Message, "OK");
                    }
                }));
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Alert", ex.Message, "OK");
        }
    }

    private void CameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        ScanedBarcode = args.Result[0].Text;
        MainThread.BeginInvokeOnMainThread(new Action(async () =>
        {
            try
            {
                ScanedBarcode = args.Result[0].Text;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }));
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        try 
        {
            WeakReferenceMessenger.Default.Send(new BarCodeMessage("Hello World"));
            try
            {
                var shell = (AppShell.Current as AppShell);
                foreach (var item in shell.Items)
                {
                    if (item.Title == "Koszyk")
                    {
                        shell.CurrentItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
            MainThread.BeginInvokeOnMainThread(new Action(async () => {
                try
                {
                    await cameraView.StopCameraAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", ex.Message, "OK");
                }
            }));
        }
        catch (Exception ex)
        {
            DisplayAlert("Alert", ex.Message, "OK");
        }
    }
}