using System.ServiceModel.Description;
using Core.Common.Core;
using Inventory.Business.Boostrapper;
using Inventory.Business.Managers;
using SM = System.ServiceModel;

namespace Inventory.ServiceHost.Console
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			System.Console.WriteLine("Starting up services...");
			System.Console.WriteLine("");

			var hostInventoryManager = new SM.ServiceHost(typeof (InventoryManager));


			StartService(hostInventoryManager, "InventoryManager");


			System.Console.WriteLine("");
			System.Console.WriteLine("Press [Enter] to exit.");
			System.Console.ReadLine();


			StopService(hostInventoryManager, "InventoryManager");
		}


		private static void StartService(SM.ServiceHost host, string serviceDescription)
		{
			host.Open();
			System.Console.WriteLine("Service {0} started.", serviceDescription);

			foreach (ServiceEndpoint serviceEndpoint in host.Description.Endpoints)
			{
				System.Console.WriteLine("Listening on endpoint:");
				System.Console.WriteLine("Address {0}", serviceEndpoint.Address);
				System.Console.WriteLine("Binding {0}", serviceEndpoint.Binding);
				System.Console.WriteLine("Contract {0}", serviceEndpoint.Contract);
			}

			System.Console.WriteLine();
		}

		private static void StopService(SM.ServiceHost host, string serviceDescription)
		{
			host.Close();
			System.Console.WriteLine("Service {0} stopped.", serviceDescription);
		}
	}
}