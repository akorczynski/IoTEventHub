using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubReceiver
{
    public class SimpleSensorData : TableEntity
    {
        public SimpleSensorData() {}

        public SimpleSensorData(string dateStamp, int sensor1, int sensor2)
        {
            this.PartitionKey = dateStamp;
            this.RowKey = Guid.NewGuid().ToString();
            DateStamp = dateStamp;
            Sensor1 = sensor1;
            Sensor2 = sensor2;
        }

        public string DateStamp { get; set; }
        public int Sensor1 { get; set; }
        public int Sensor2 { get; set; }
    }
}
