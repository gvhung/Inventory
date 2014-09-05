using System.ComponentModel.Composition;
using System.ServiceModel;
using Inventory.Client.Contracts;
using Inventory.Client.Entities;
using Inventory.Data.Contracts.DTOs;

namespace Inventory.Client.Proxies
{
	[Export(typeof (IInventoryService))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class InventoryClient : ClientBase<IInventoryService>, IInventoryService
	{
		public InventoryClient()
		{
			int i = 0;
		}
		
		public ProductDto GetProduct(int productId)
		{
			return Channel.GetProduct(productId);
		}

		public ProductDto[] GetAllProducts()
		{
			return Channel.GetAllProducts();
		}

		public CategoryProductDto[] GetActiveProducts(string letter, int? page, int pageSize)
		{
			return Channel.GetActiveProducts(letter, page, pageSize);
		}

		public void DeleteProduct(int productId)
		{
			Channel.DeleteProduct(productId);
		}

		public ProductDto UpdateProduct(ProductDto product)
		{
			return Channel.UpdateProduct(product);
		}

		public ProductDto AddProduct(ProductDto product)
		{
			return Channel.AddProduct(product);
		}

		public CategoryDto GetCategory(int categoryId)
		{
			return Channel.GetCategory(categoryId);
		}

		public CategoryDto[] GetAllCategories()
		{
			return Channel.GetAllCategories();
		}

		public void DeleteCategory(int categoryId)
		{
			Channel.DeleteCategory(categoryId);
		}

		public CategoryDto UpdateCategory(CategoryDto category)
		{
			return Channel.UpdateCategory(category);
		}

		public CategoryDto AddCategory(CategoryDto category)
		{
			return Channel.AddCategory(category);
		}

		public int GetNumberOfActiveProducts(string letter)
		{
			return Channel.GetNumberOfActiveProducts(letter);
		}
	}
}