using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace PapayagramsServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Contracts.ChatServiceImplementation)))
            {
                host.Open();
                Console.WriteLine("Server running...");
                Console.ReadLine();
            }
        }
    }
}
