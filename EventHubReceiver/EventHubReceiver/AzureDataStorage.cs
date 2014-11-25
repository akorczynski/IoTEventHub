using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure;

namespace EventHubReceiver
{
    public class AzureDataStorage
    {
        private CloudStorageAccount _StorageAccount;
        private CloudTableClient _TableClient;
        private CloudTable _CloudTable;

        public AzureDataStorage()
        {
            _StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            _TableClient = _StorageAccount.CreateCloudTableClient();
            _CloudTable = _TableClient.GetTableReference("IoTData");
            _CloudTable.CreateIfNotExists();
        }

        public void SaveData(string dateStamp, int sensor1, int sensor2)
        {
            var ssd = new SimpleSensorData(dateStamp, sensor1, sensor2);
            var insertOperation = TableOperation.Insert(ssd);
            _CloudTable.Execute(insertOperation);
        }
    }
}
