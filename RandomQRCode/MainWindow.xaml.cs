using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using QRCoder;
using System.Drawing;
using System.IO;

namespace RandomQRCode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public Random Random { get; } = new Random();

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                while (true)
                {
                    await Dispatcher.InvokeAsync(
                        () => QRImage.Source = BitmapToImageSource(GenerateQRCode(Random.Next().ToString())));
                    await Task.Delay(10000);
                }
            });
        }

        public Bitmap GenerateQRCode(string data)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void QRImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            QRImage.Source = BitmapToImageSource(GenerateQRCode(Random.Next().ToString()));
        }
    }
}
