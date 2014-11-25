using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Threading;

namespace EventHubSender
{
    public class Program
    {
        private static string eventHubName = "evthubs";
        private static string connectionString = "x";
        private static Random _Random;

        public static void Main(string[] args)
        {
            _Random = new Random();
            SendingRandomMessages().Wait();
        }

        private static async Task SendingRandomMessages()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            while (true)
            {
                try
                {
                    var message = DateTime.Now.ToString(" HH:mm:ss.fff") + "," + SensorData(0) + "," + SensorData(1);
                    Console.WriteLine(message);
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} ERROR: {1}", DateTime.Now.ToString(), exception.Message);
                    Console.ResetColor();
                }
                await Task.Delay(200);
            }
        }

        private static int[] _SensorValue = new int[2] {10, 50};
        private static int SensorData(int index)
        {
            var addOn = _Random.Next(-1, 2);
            _SensorValue[index] += addOn;
            return _SensorValue[index];
        }
    }
}
