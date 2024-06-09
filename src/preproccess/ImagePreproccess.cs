using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace treelilnick.preproccess
{
    public class ImagePreproccess

    {
        public static BitmapSource ConvertToGrayscale(BitmapSource originalImage)
        {
            PixelFormat format = originalImage.Format;
            if (format == PixelFormats.Gray8 || format == PixelFormats.Gray16 || format == PixelFormats.Indexed8)
            {
                return originalImage;
            }
            int width = originalImage.PixelWidth;
            int height = originalImage.PixelHeight;
            int bytesPerPixel = 4;
            int stride = width * bytesPerPixel;

            byte[] pixels = new byte[height * stride];
            originalImage.CopyPixels(pixels, stride, 0);

            byte[] grayscalePixels = new byte[height * width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x * bytesPerPixel;
                    byte blue = pixels[index];
                    byte green = pixels[index + 1];
                    byte red = pixels[index + 2];

                    byte gray = (byte)(red * 0.3 + green * 0.59 + blue * 0.11);

                    grayscalePixels[y * width + x] = gray;
                }
            }

            return BitmapSource.Create(width, height, originalImage.DpiX, originalImage.DpiY, PixelFormats.Gray8, null, grayscalePixels, width);
        }
        public static List<string> ConvertToASCII(BitmapSource grayscaleImage)
        {
            List<string> result = [];
            int width = grayscaleImage.PixelWidth;
            int height = grayscaleImage.PixelHeight;
            int paddedWidth = (width + 7) / 8;

            byte[] grayscalePixels = new byte[height * width];
            grayscaleImage.CopyPixels(grayscalePixels, width, 0);

            byte[] binaryPixels = new byte[height * paddedWidth];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int grayscaleIndex = y * width + x;
                    int bitIndex = 7 - (x % 8);

                    byte grayscaleValue = grayscalePixels[grayscaleIndex];
                    byte thresholdValue = grayscaleValue <= 122 ? (byte)1 : (byte)0;
                    binaryPixels[y * paddedWidth + x / 8] |= (byte)(thresholdValue << bitIndex);
                }
            }

            for (int i = 0; i < height; i++)
            {
                StringBuilder temp = new();
                for (int j = 0; j < paddedWidth; j++)
                {
                    temp.Append((char)binaryPixels[i * paddedWidth + j]);
                }
                result.Add(temp.ToString());
            }
            return result;
        }

        public static string ConvertToASCIIString(BitmapSource grayscaleImage)
        {
            int width = grayscaleImage.PixelWidth;
            int height = grayscaleImage.PixelHeight;
            int paddedWidth = (width + 7) / 8;

            byte[] grayscalePixels = new byte[height * width];
            grayscaleImage.CopyPixels(grayscalePixels, width, 0);

            byte[] binaryPixels = new byte[height * paddedWidth];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int grayscaleIndex = y * width + x;
                    int bitIndex = 7 - (x % 8);

                    byte grayscaleValue = grayscalePixels[grayscaleIndex];
                    byte thresholdValue = grayscaleValue <= 122 ? (byte)1 : (byte)0;
                    binaryPixels[y * paddedWidth + x / 8] |= (byte)(thresholdValue << bitIndex);
                }
            }
            StringBuilder res = new();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < paddedWidth; j++)
                {
                    res.Append((char)binaryPixels[i * paddedWidth + j]);
                }
            }
            return res.ToString();
        }
    }
}