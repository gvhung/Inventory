using System.ComponentModel.Composition.Hosting;
using Inventory.Business.Managers;
using Inventory.Data.Data_Repositories;

namespace Inventory.Business.Boostrapper
{
	public static class MEFLoader
	{
		public static CompositionContainer Init()
		{
			var catalog = new AggregateCatalog();

			catalog.Catalogs.Add(new AssemblyCatalog(typeof(InventoryManager).Assembly));
			catalog.Catalogs.Add(new AssemblyCatalog(typeof(ProductRepository).Assembly));
			
			var container = new CompositionContainer(catalog);

			return container;
		}
	}
}