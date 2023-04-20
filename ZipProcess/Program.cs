using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ZipProcess
{
    internal class Program
    {
        async public static Task Main(String[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Not enough arguments");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            try
            {
                await CompressFile(args[0]);
            }
            catch (UnauthorizedAccessException ex)
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

        async private static Task CompressFile(string originalFileName)
        {
            string compressedFileName = originalFileName + ".zip";

            using (FileStream originalFileStream = File.Open(originalFileName, FileMode.Open))
            {
                using (FileStream compressedFileStream = File.Create(compressedFileName))
                {
                    using (var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        await originalFileStream.CopyToAsync(compressor);
                    }
                }
            }
        }
    }
}
