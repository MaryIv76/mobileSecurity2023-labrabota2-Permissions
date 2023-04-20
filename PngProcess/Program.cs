using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PngProcess
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Not enough arguments");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            try
            {
                ConvertImageToPng(args[0]);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(-5);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(-5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }

        private static void ConvertImageToPng(string originalFileName)
        {
            FileInfo fileInfo = new FileInfo(originalFileName);
            if (!fileInfo.Extension.Equals(".jpg") && !fileInfo.Extension.Equals(".jpeg") && !fileInfo.Extension.Equals(".bmp") && !fileInfo.Extension.Equals(".tiff") && !fileInfo.Extension.Equals(".gif"))
            {
                Console.WriteLine("Not an image file");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            string pngFileName = fileInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(originalFileName) + ".png";

            Bitmap myBitmap = new Bitmap(originalFileName);
            myBitmap.Save(pngFileName, ImageFormat.Png);
            myBitmap.Dispose();
        }
    }
}
