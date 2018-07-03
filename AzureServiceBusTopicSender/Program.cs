using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureTopicCommon;
namespace AzureServiceBusTopicSender
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureTpoic.SendMessage();

            Console.ReadLine();
        }
    }
}
