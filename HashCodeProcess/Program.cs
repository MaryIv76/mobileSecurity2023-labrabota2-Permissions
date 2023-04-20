using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HashCodeProcess
{
    internal class Program
    {
        async public static Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Not enough arguments");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            try
            {
                await GetFileHashCode(args[0]);
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

        async private static Task GetFileHashCode(string originalFileName)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                FileInfo fileInfo = new FileInfo(originalFileName);
                string hashCodeFileName = fileInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(originalFileName) + "_hash.txt";

                using (FileStream fileStream = fileInfo.Open(FileMode.Open))
                {
                    using (var outputStream = new FileStream(hashCodeFileName, FileMode.Create))
                    {
                        fileStream.Position = 0;
                        byte[] hashValue = mySHA256.ComputeHash(fileStream);
                        await outputStream.WriteAsync(hashValue, 0, hashValue.Length);
                    }
                }
            }
        }
    }
}
