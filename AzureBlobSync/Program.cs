using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using PowerArgs;

namespace AzureBlobSync
{
    internal class Program
    {
        private static void Main(string[] argsStd)
        {
            CliArgs args;

            try
            {
                args = Args.Parse<CliArgs>(argsStd);
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<CliArgs>());
                return;
            }

            var storageAccount = args.DevMode ? CloudStorageAccount.DevelopmentStorageAccount : CloudStorageAccount.Parse(args.ConnString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(args.Container);
            container.CreateIfNotExists();

            if (args.Dir == ".") args.Dir = Directory.GetCurrentDirectory();

            var files = Directory.GetFiles(args.Dir);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                var blob = container.GetBlockBlobReference(fileName);

                if (blob.Exists())
                {
                    if (args.Verbose) Console.WriteLine("Skipping {0} as already exists...", fileName);
                    continue;
                }

                Console.WriteLine("Uploading {0}...", fileName);
                blob.UploadFromFile(file, FileMode.Open);
            }
        }
    }
}
