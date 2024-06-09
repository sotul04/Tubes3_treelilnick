using System.IO;
using System.Windows.Media.Imaging;

namespace treelilnick.imageloader
{
    public class ImageLoader
    {
        public static BitmapSource LoadImage(string filePath)
        {
            // Create a BitmapImage and set its properties
            // BitmapImage bitmapImage = new();

            // using (FileStream stream = new(filePath, FileMode.Open, FileAccess.Read))
            // {
            //     bitmapImage.BeginInit();
            //     bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //     bitmapImage.UriSource = null;
            //     bitmapImage.StreamSource = stream;
            //     bitmapImage.EndInit();
            // }
            // bitmapImage.Freeze();
            // return bitmapImage;
            Uri imgUri = new Uri(filePath, UriKind.Relative);
            BmpBitmapDecoder decoder2 = new BmpBitmapDecoder(imgUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];
            return bitmapSource2;
        }
    }
}