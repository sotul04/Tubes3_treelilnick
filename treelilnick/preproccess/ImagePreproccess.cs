using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace treelilnick.preproccess
{
    public class ImagePreproccess
    {
        public static BitmapSource ConvertToGrayscale(BitmapSource originalImage)
        {
            int width = originalImage.PixelWidth;
            int height = originalImage.PixelHeight;
            int stride = width * 4; // 4 bytes

            byte[] pixels = new byte[height * stride];
            originalImage.CopyPixels(pixels, stride, 0);

            byte[] grayscalePixels = new byte[height * stride];

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte gray = (byte)(pixels[i] * 0.3 + pixels[i + 1] * 0.59 + pixels[i + 2] * 0.11);

                grayscalePixels[i] = gray; // Red 
                grayscalePixels[i + 1] = gray; // Green 
                grayscalePixels[i + 2] = gray; // Blue 
                grayscalePixels[i + 3] = pixels[i + 3]; // Alpha 
            }

            return BitmapSource.Create(width, height, originalImage.DpiX, originalImage.DpiY, PixelFormats.Bgra32, null, grayscalePixels, stride);
        }

        public BitmapSource ApplyThreshold(BitmapSource grayscaleImage)
        {
            int width = grayscaleImage.PixelWidth;
            int height = grayscaleImage.PixelHeight;
            int stride = (width + 7) / 8; 

            byte[] grayscalePixels = new byte[height * stride];
            grayscaleImage.CopyPixels(grayscalePixels, stride, 0);

            byte[] binaryPixels = new byte[height * stride];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int grayscaleIndex = y * stride + x / 8;
                    int bitIndex = 7 - (x % 8);

                    byte grayscaleValue = (byte)((grayscalePixels[grayscaleIndex] >> bitIndex) & 0x01);
                    byte thresholdValue = grayscaleValue <= 122 ? (byte)0 : (byte)1;

                    binaryPixels[grayscaleIndex] |= (byte)(thresholdValue << bitIndex);
                }
            }
            return BitmapSource.Create(width, height, grayscaleImage.DpiX, grayscaleImage.DpiY, PixelFormats.BlackWhite, null, binaryPixels, stride);
        }
    }

}
