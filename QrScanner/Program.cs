using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Timers;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.QrCode;
using Timer = System.Timers.Timer;

class Program
{
    static FilterInfoCollection filterInfoCollection;
    static VideoCaptureDevice captureDevice;
    static Bitmap QrBitmap { get; set; }

    static void Main(string[] args)
    {
        filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

        VideoCaptureDevice captureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
        captureDevice.NewFrame += CaptireDevice_NewFrame;
        captureDevice.Start();
        Timer timer = new Timer(500);
        timer.Elapsed += TimedEvent;
        timer.Enabled = true;
        timer.Start();

    }

    private static void CaptireDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
        QrBitmap = (Bitmap)eventArgs.Frame.Clone();
    }

    private static void TimedEvent(object source, ElapsedEventArgs e)
    {
        try
        {
            if (QrBitmap != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode(QrBitmap);
                if (result != null)
                {
                    OpenUrlInBrowser(result.ToString());
                    QrBitmap = null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при чтении QR: " + ex.Message);
        }
    }

    static void OpenUrlInBrowser(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при открытии URL: " + ex.Message);
        }
    }
}
