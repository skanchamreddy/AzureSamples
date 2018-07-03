using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureTopicCommon;
namespace AzureServiceBusTopicReceiver2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            AzureTpoic.ReceiveMessage("Topic1_2");
           
            Console.ReadLine();
        }
    }
}
