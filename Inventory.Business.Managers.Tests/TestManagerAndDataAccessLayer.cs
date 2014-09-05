using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Core.Common.Core;
using Inventory.Business.Boostrapper;
using Inventory.Data.Contracts.DTOs;
using NUnit.Framework;

namespace Inventory.Business.Managers.Tests
{
	[TestFixture]
	public class TestManagerAndDataAccessLayer
	{
		[TestFixtureSetUp]
		public void Initalize()
		{
			ObjectBase.Container = MEFLoader.Init();
			ObjectBase.Container.SatisfyImportsOnce(this);
		}

		[Test]
		public void TestAddProduct()
		{
			var inventoryManager = new InventoryManager();
			ProductDto[] allProducts = inventoryManager.GetAllProducts();
			int productsNumber = allProducts.Length;
			var productDto = new ProductDto
			                 {
				                 Name = "Test",
				                 Archived = false,
				                 Price = 99.99
			                 };
			ProductDto result = inventoryManager.AddProduct(productDto);
			allProducts = inventoryManager.GetAllProducts();
			Assert.IsTrue(productsNumber + 1 == allProducts.Length);
			Assert.IsTrue(result.Price == productDto.Price);
			Assert.IsTrue(result.Name == productDto.Name);
			Assert.IsTrue(result.Archived == productDto.Archived);
		}

		[Test]
		public void TestAddProductAndCategory()
		{
			var inventoryManager = new InventoryManager();
			ProductDto[] allProducts = inventoryManager.GetAllProducts();
			var productDto = new ProductDto
			                 {
				                 Name = "Test",
				                 Archived = false,
				                 Price = 99.99
			                 };
			CategoryDto[] allCategories = inventoryManager.GetAllCategories();
			productDto.Categories.Add(allCategories[0]);
			
			ProductDto result = inventoryManager.AddProduct(productDto);

			ProductDto reload = inventoryManager.GetProduct(result.ProductId);

			Assert.IsTrue(reload.Categories[0].CategoryId == allCategories[0].CategoryId);
		}

		[Test]
		public void TestDeleteProduct()
		{
			var inventoryManager = new InventoryManager();
			ProductDto[] allProducts = inventoryManager.GetAllProducts();
			if (allProducts.Length > 0)
			{
				ProductDto productToDelete = allProducts[0];
				inventoryManager.DeleteProduct(productToDelete.ProductId);

				ProductDto productDto = inventoryManager.GetProduct(productToDelete.ProductId);
				Assert.IsNull(productDto);
			}
		}

		[Test]
		public void TestLoadingHibernateConfig()
		{
			var inventoryManager = new InventoryManager();
			ProductDto[] allProducts = inventoryManager.GetAllProducts();
			CategoryDto[] allCategories = inventoryManager.GetAllCategories();
		}

		[Test]
		public void TestUpdateProductSetAllUnArchived()
		{
			var inventoryManager = new InventoryManager();
			ProductDto[] allProducts = inventoryManager.GetAllProducts();

			foreach (ProductDto product in allProducts)
			{
				if (product.Archived)
				{
					product.Archived = false;
					inventoryManager.UpdateProduct(product);
				}
			}
			CategoryProductDto[] activeProducts = inventoryManager.GetActiveProducts(null,0,10);
			Assert.IsNull(activeProducts.Length == allProducts.Length);
		}

		[Test]
		public void TestUpdateProductSetArchived()
		{
			var inventoryManager = new InventoryManager();
			CategoryProductDto[] allProducts = inventoryManager.GetActiveProducts(null,0,10);
			if (allProducts.Length > 0)
			{
				CategoryProductDto productToUpdate = allProducts[0];
				productToUpdate.ProductName = "New Name";
				productToUpdate.Price = 88.88;
				productToUpdate.Archived = true;
				//inventoryManager.UpdateProduct(productToUpdate);

				ProductDto productDto = inventoryManager.GetProduct(productToUpdate.ProductId);
				Assert.IsTrue(productDto.Name == productToUpdate.ProductName);
				Assert.IsTrue(productDto.Price == productToUpdate.Price);
				Assert.IsTrue(productDto.Archived == productToUpdate.Archived);

				CategoryProductDto[] activeProducts = inventoryManager.GetActiveProducts(null,0,10);
				IEnumerable<CategoryProductDto> result = from product in activeProducts
					where
						product.ProductId == productToUpdate.ProductId
					select product;
				CategoryProductDto firstOrDefault = result.FirstOrDefault();
				Assert.IsNull(firstOrDefault);
			}
		}
	}
}