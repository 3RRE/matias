using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CapaNegocio.Queue.Client {
    public class AzureQueueClient {
        private static readonly Lazy<AzureQueueClient> instance = new Lazy<AzureQueueClient>(() => new AzureQueueClient());
        public static AzureQueueClient Instance => instance.Value;

        private readonly Dictionary<string, CloudQueue> queues;

        public AzureQueueClient() {
            queues = new Dictionary<string, CloudQueue>();
            string connectionString = ConfigurationManager.AppSettings["AzureQueueConnectionString"];

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            string azureQueueWhatsAppName = ConfigurationManager.AppSettings["AzureQueueWhatsAppName"];
            CloudQueue whatsappQueue = queueClient.GetQueueReference(azureQueueWhatsAppName);
            whatsappQueue.CreateIfNotExists();
            queues.Add(azureQueueWhatsAppName, whatsappQueue);

            string azureQueueEmailName = ConfigurationManager.AppSettings["AzureQueueEmailName"];
            CloudQueue emailQueue = queueClient.GetQueueReference(azureQueueEmailName);
            emailQueue.CreateIfNotExists();
            queues.Add(azureQueueEmailName, emailQueue);
        }

        public async Task SendMessageAsync(string queueName, object message) {
            if(queues.TryGetValue(queueName, out CloudQueue queue)) {
                string serializedMessage = JsonConvert.SerializeObject(message);
                CloudQueueMessage queueMessage = new CloudQueueMessage(serializedMessage);
                await queue.AddMessageAsync(queueMessage);
            } else {
                throw new ArgumentException($"Queue '{queueName}' not found.");
            }
        }
    }
}
