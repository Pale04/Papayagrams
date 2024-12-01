using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;

namespace PapayagramsServer
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Contracts.ServiceImplementation)))
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                host.Open();
                Console.WriteLine("Server running...");
                Console.ReadLine();
            }
        }
    }
}
