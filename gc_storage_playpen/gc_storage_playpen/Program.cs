using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Services;
using Google.Apis.Storage.v1;

namespace gc_storage_playpen
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = new Task(DoAsync);
            t.Start();

            Console.WriteLine("doing requests...");
            Console.ReadLine();
        }

        static async void DoAsync()
        {

            ListBuckets("mq-cloud-prototyping");


        }

        static StorageService CreateStorageClient()
        {
            //var credentials = Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync().Result;

            //if (credentials.IsCreateScopedRequired)
            //{
            //    credentials = credentials.CreateScoped(new[] { StorageService.Scope.DevstorageFullControl });
            //}

            var serviceInitializer = new BaseClientService.Initializer()
            {
                ApplicationName = "Storage Sample",
                HttpClientInitializer = GetServiceCredential()
            };

            return new StorageService(serviceInitializer);
        }

        static ServiceAccountCredential GetServiceCredential()
        {
            //setup ouath

            var credentials = Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync().Result;

            var serviceAccountEmail = "43059975295-3h4tar526eql3orbheq3aa7mkfo1735l@developer.gserviceaccount.com"; //mq-cloud-prototyping
            var key = "key.p12";
            
            string O_AUTH_STORE_SCOPE = "https://www.googleapis.com/auth/devstorage.read_write";

            var certificate = new X509Certificate2(key, "notasecret", X509KeyStorageFlags.Exportable);

            return new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   Scopes = new[] { O_AUTH_STORE_SCOPE }
               }.FromCertificate(certificate));      
        }

        static  void ListBuckets(string projectId)
        {
            StorageService storage = CreateStorageClient();

            var buckets = storage.Buckets.List(projectId).Execute();

            if (buckets.Items != null)
            {
                foreach (var bucket in buckets.Items)
                {
                    Console.WriteLine($"Bucket: {bucket.Name}");
                }
            }
        }
    }
}
