﻿using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PowerArgs;

namespace AzureBlobSync
{
    class Program
    {
        private static readonly MD5 MD5 = MD5.Create();

        static void Main(string[] argsStd)
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

            if (args.Directory == ".") args.Directory = Directory.GetCurrentDirectory();

            var files = Directory.GetFiles(args.Directory);

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