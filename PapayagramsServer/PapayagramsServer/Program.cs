using System;
using System.ServiceModel;

namespace PapayagramsServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Contracts.ServiceImplementation)))
            {
                host.Open();
                Console.WriteLine("Server running...");
                Console.ReadLine();
            }
        }
    }
}
