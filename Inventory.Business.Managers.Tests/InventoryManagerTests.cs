using AutoMapper;
using Core.Common.Contracts;
using Inventory.Client.Entities;
using Inventory.Data.Contracts.DTOs;
using Inventory.Data.Contracts.Repository_Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Inventory.Business.Managers.Tests
{
	[TestFixture]
	public class InventoryManagerTests
	{
		[TestFixtureSetUp]
		public void Initialize()
		{
			Mapper.CreateMap<Product, ProductDto>();
			Mapper.CreateMap<ProductDto, Product>();
			Mapper.CreateMap<Category, CategoryDto>();
			Mapper.CreateMap<CategoryDto, Category>();
		}

			
		[Test]
		public void TestProductAddNew()
		{
			Product newProduct = new Product();
			Product addedProduct = new Product() { ProductId = 1 };
			ProductDto newProductDto = Mapper.Map<ProductDto>(newProduct);
			ProductDto addedProductDto = Mapper.Map<ProductDto>(addedProduct);
			
			IDataRepositoryFactory mockedRepositoryFactory = Substitute.For<IDataRepositoryFactory>();
			mockedRepositoryFactory.GetDataRepository<IProductRepository>().Add(newProduct).Returns(addedProduct);

		
			InventoryManager manager = new InventoryManager(mockedRepositoryFactory);
			ProductDto results = manager.AddProduct(newProductDto);

			//cannot test InventoryManager with Fake Repository, because of Mapper
			Assert.IsTrue(results == addedProductDto);
		}

		[Test]
		public void UpdateProduct_update_existing()
		{
			Product existingProduct = new Product() { ProductId = 1 };
			Product updatedProduct = new Product() { ProductId = 1 , Name = "Updated Product"};
			
			IDataRepositoryFactory mockedRepositoryFactory = Substitute.For<IDataRepositoryFactory>();
			mockedRepositoryFactory.GetDataRepository<IProductRepository>().Update(existingProduct).Returns(updatedProduct);

			InventoryManager manager = new InventoryManager(mockedRepositoryFactory);
			ProductDto results = manager.UpdateProduct(Mapper.Map<ProductDto>(existingProduct));

			Assert.IsTrue(results.Name == updatedProduct.Name);
		}
	}
}
