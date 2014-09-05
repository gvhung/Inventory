using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Inventory.Client.Proxies;
using Inventory.Data.Data_Repositories;

namespace Inventory.Client.Bootstrapper
{
	public static class MEFLoader
	{
		public static CompositionContainer Init()
		{
			return Init(null);
		}

		public static CompositionContainer Init(ICollection<ComposablePartCatalog> catalogParts)
		{
			var catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog(typeof (InventoryClient).Assembly));
			catalog.Catalogs.Add(new AssemblyCatalog(typeof (ProductRepository).Assembly));
			
			if (catalogParts != null)
			{
				foreach (ComposablePartCatalog catalogPart in catalogParts)
				{
					catalog.Catalogs.Add(catalogPart);
				}
			}

			var container = new CompositionContainer(catalog);
			return container;
		}
	}
}