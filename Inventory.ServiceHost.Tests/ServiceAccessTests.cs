using System.ServiceModel;
using Inventory.Client.Contracts;
using NUnit.Framework;

namespace Inventory.ServiceHost.Tests
{
	[TestFixture]
	public class ServiceAccessTests
	{
		[Test]
		public void TestInventoryManagerAsService()
		{
			var channelFactory =
				new ChannelFactory<IInventoryService>("");

			IInventoryService proxy = channelFactory.CreateChannel();

			(proxy as ICommunicationObject).Open();

			channelFactory.Close();
		}
	}
}