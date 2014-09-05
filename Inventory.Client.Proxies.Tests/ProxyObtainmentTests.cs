using Core.Common.Contracts;
using Core.Common.Core;
using Inventory.Client.Bootstrapper;
using Inventory.Client.Contracts;
using Inventory.Client.Entities;
using Inventory.Data.Contracts.DTOs;
using NUnit.Framework;

namespace Inventory.Client.Proxies.Tests
{
	[TestFixture]
	public class ProxyObtainmentTests
	{
		[TestFixtureSetUp]
		public void Initialize()
		{
			ObjectBase.Container = MEFLoader.Init();
		}

		[Test]
		public void ObtainProxyFromContainerUsingServiceContract()
		{
			var proxy = ObjectBase.Container.GetExportedValue<IInventoryService>();

			Assert.IsTrue(proxy is InventoryClient);
			ProductDto addedProduct = new ProductDto() { Name = "Test new PRoduct", Archived = false,Price = 99.99};
			ProductDto result = proxy.AddProduct(addedProduct);
			ProductDto[] allProducts = proxy.GetAllProducts();
		}

		[Test]
		public void ObtainProxyFromServiceFactory()
		{
			IServiceFactory factory = new ServiceFactory();
			var proxy = factory.CreateClient<IInventoryService>();

			Assert.IsTrue(proxy is InventoryClient);
		}

		[Test]
		public void ObtainServiceFactoryAnProxyFromContainer()
		{
			var factory = ObjectBase.Container.GetExportedValue<IServiceFactory>();
			var proxy = factory.CreateClient<IInventoryService>();

			Assert.IsTrue(proxy is InventoryClient);
		}
	}
}