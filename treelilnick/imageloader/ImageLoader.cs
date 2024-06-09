using System.IO;
using System.Windows.Media.Imaging;

namespace treelilnick.imageloader
{
    public class ImageLoader
    {
        public static BitmapSource LoadImage(string filePath)
        {
            Uri imgUri = new Uri(filePath, UriKind.Relative);
            BmpBitmapDecoder decoder2 = new BmpBitmapDecoder(imgUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];
            return bitmapSource2;
        }
    }
}