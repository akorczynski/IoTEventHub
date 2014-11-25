using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;

namespace EventHubReceiver
{
    public class SimpleEventProcessor : IEventProcessor
    {
        private Stopwatch _CheckpointStopWatch;
        private AzureDataStorage _AzureDataStorage;

        public SimpleEventProcessor()
        {
            _AzureDataStorage = new AzureDataStorage();
        }

        async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine(string.Format("Processor Shuting Down.  Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason.ToString()));
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            Console.WriteLine(string.Format("SimpleEventProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset));
            _CheckpointStopWatch = new Stopwatch();
            _CheckpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                string data = Encoding.UTF8.GetString(eventData.GetBytes());

                Console.WriteLine(string.Format("Message received.  Partition: '{0}', Data: '{1}'",
                    context.Lease.PartitionId, data));
                var d = data.Split(',');
                if (d.Length == 3)
                {
                    _AzureDataStorage.SaveData(d[0], int.Parse(d[1]), int.Parse(d[2]));
                }
            }

            if (_CheckpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                lock (this)
                {
                    _CheckpointStopWatch.Reset();
                }
            }
        }
    }
}
