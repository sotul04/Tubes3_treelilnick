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
                    byte thresholdValue = grayscaleValue <= 128 ? (byte)1 : (byte)0;
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
        public static string ConvertToASCIIPattern(BitmapSource grayscaleImage)
        {
            int width = grayscaleImage.PixelWidth;
            int height = grayscaleImage.PixelHeight;
            int bitTakes = 40;

            byte[] grayscalePixels = new byte[height * width];
            grayscaleImage.CopyPixels(grayscalePixels, width, 0);

            int minDiff = bitTakes;
            byte[] result = new byte[bitTakes / 8];

            for (int i = height / 4; i < height * 3 / 4; i++)
            {
                // skip first two
                int j = 0;
                int whiteCnt = 0;
                int blackCnt = 0;
                // skip if black count < 3
                for (; j < width - bitTakes; j += 8)
                {
                    int blk = 0;
                    for (int k = 0; k < 8; k++)
                    {
                        if (grayscalePixels[i * width + j + k] <= 128) blk++;
                    }
                    if (blk > 2) break;
                }
                // pick 32 bit
                byte[] currBit = new byte[bitTakes / 8];
                for (int k = 0; k < bitTakes; k++)
                {
                    int val;
                    if (j + k >= width)
                    {
                        blackCnt++;
                        val = 1;
                    }
                    else if (grayscalePixels[i * width + j + k] <= 128)
                    {
                        blackCnt++;
                        val = 1;
                    }
                    else
                    {
                        whiteCnt++;
                        val = 0;
                    }
                    int bitIdx = 7 - k % 8;
                    currBit[k / 8] |= (byte)(val << bitIdx);
                }
                int diff = Math.Abs(whiteCnt - blackCnt);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    for (int k = 0; k < bitTakes / 8; k++)
                    {
                        result[k] = currBit[k];
                    }
                }
            }
            StringBuilder res = new();
            for (int i = 0; i < bitTakes / 8; i++)
            {
                res.Append((char)result[i]);
            }
            return res.ToString();
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
                    byte thresholdValue = grayscaleValue <= 128 ? (byte)1 : (byte)0;
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