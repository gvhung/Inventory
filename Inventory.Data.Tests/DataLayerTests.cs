using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Contracts;
using Core.Common.Core;
using Inventory.Business.Boostrapper;
using Inventory.Client.Entities;
using Inventory.Data.Contracts.DTOs;
using Inventory.Data.Contracts.Repository_Interfaces;
using NHibernate;
using NSubstitute;
using NUnit.Framework;

namespace Inventory.Data.Tests
{
	[TestFixture]
	public class DataLayerTests
	{
		[TestFixtureSetUp]
		public void Initialize()
		{
			ObjectBase.Container = MEFLoader.Init();
		}

		[Test]
		public void test_factory_mocking1()
		{
			var products = new List<Product>
			           {
				           new Product {ProductId = 1, Name = "Mustang"},
				           new Product {ProductId = 2, Name = "Corvette"}
			           };
			Product[] array = products.ToArray();
			var mockedDataRepository = Substitute.For<IDataRepositoryFactory>();
			mockedDataRepository.GetDataRepository<IProductRepository>().GetAll().Returns(array);

			var repositoryFactory = new RepositoryFactoryTestClass(mockedDataRepository);
			Product[] productsReturn = repositoryFactory.GetProducts();
			Assert.IsTrue(array == productsReturn);
		}

		[Test]
		public void test_factory_mocking2()
		{
			var products = new List<Product>
			           {
				           new Product {ProductId = 1, Name = "Mustang"},
				           new Product {ProductId = 2, Name = "Corvette"}
			           };
			Product[] array = products.ToArray();
			var mockProductRepository = Substitute.For<IProductRepository>();
			mockProductRepository.GetAll().Returns(array);


			var mockedDataRepository = Substitute.For<IDataRepositoryFactory>();
			mockedDataRepository.GetDataRepository<IProductRepository>().Returns(mockProductRepository);

			var repositoryFactory = new RepositoryFactoryTestClass(mockedDataRepository);
			Product[] productsReturn = repositoryFactory.GetProducts();
			Assert.IsTrue(array == productsReturn);
		}

		[Test]
		public void test_repository_factory_usage()
		{
			var factoryTest = new RepositoryFactoryTestClass();
			Product[] products = factoryTest.GetProducts();
			Assert.IsTrue(products != null);
		}

		[Test]
		public void test_repository_mocking()
		{
			var products = new List<Product>
			           {
				           new Product {ProductId = 1, Name = "Mustang"},
				           new Product {ProductId = 2, Name = "Corvette"}
			           };
			Product[] array = products.ToArray();
			var mockedProductRepository = Substitute.For<IProductRepository>();
			mockedProductRepository.GetAll().Returns(products.ToArray());


			var repositoryTest = new RepositoryTestClass(mockedProductRepository);
			Product[] retProducts = repositoryTest.GetProducts();
			Assert.IsTrue(retProducts == array);
		}

		[Test]
		public void test_repository_usage()
		{
			var repositoryTest = new RepositoryTestClass();
			IEnumerable<Product> Products = repositoryTest.GetProducts();

			Assert.IsTrue(Products != null);
		}
	}

	public class RepositoryTestClass
	{
		[Import]
		private  IProductRepository _productRepository;

		public RepositoryTestClass()
		{
			ObjectBase.Container.SatisfyImportsOnce(this);
		}

		public RepositoryTestClass(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public Product[] GetProducts()
		{
			Product[] products = _productRepository.GetAll();
			return products;
		}
	}

	public class RepositoryFactoryTestClass
	{
		[Import]
		private  IDataRepositoryFactory _dataRepositoryFactory;

		public RepositoryFactoryTestClass()
		{
			ObjectBase.Container.SatisfyImportsOnce(this);
			ISessionFactory mockedSessionFactory = Substitute.For<ISessionFactory>();
			ISession session = Substitute.For<ISession>();
			mockedSessionFactory.OpenSession().Returns(session);
			ObjectBase.Container.ComposeExportedValue(Substitute.For<ISessionFactory>());
		}

		public RepositoryFactoryTestClass(IDataRepositoryFactory factory)
		{
			_dataRepositoryFactory = factory;
		}

		public Product[] GetProducts()
		{
			var productRepository = _dataRepositoryFactory.GetDataRepository<IProductRepository>();
			return productRepository.GetAll();
		}
	}
}