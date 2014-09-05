using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Core.Common.Contracts;
using Core.Common.Core;
using Inventory.Client.Bootstrapper;
using Inventory.Client.Contracts;
using Inventory.Client.Entities;
using Inventory.Data.Contracts.DTOs;
using Inventory.Data.Contracts.Repository_Interfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace Inventory.Business.Managers
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
		ConcurrencyMode = ConcurrencyMode.Multiple,
		ReleaseServiceInstanceOnTransactionComplete = false, IncludeExceptionDetailInFaults = true)
	]
	public class InventoryManager : ManagerBase, IInventoryService
	{
		private readonly ISessionFactory _sessionFactory;

		[Import] 
		private IDataRepositoryFactory _dataRepositoryFactory;

		public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
		{
			_dataRepositoryFactory = dataRepositoryFactory;
		}


		public InventoryManager()
		{
			ObjectBase.Container = MEFLoader.Init();
			ObjectBase.Container.SatisfyImportsOnce(this);
			//mapping 
			Mapper.CreateMap<Product, ProductDto>();
			Mapper.CreateMap<ProductDto, Product>();
			Mapper.CreateMap<Category, CategoryDto>();
			Mapper.CreateMap<CategoryDto, Category>();

			var cfg = new Configuration();
			cfg.Configure("hibernate.cfg.xml");
			_sessionFactory = cfg.CurrentSessionContext<WcfOperationSessionContext>().BuildSessionFactory();

			ObjectBase.Container.ComposeExportedValue(_sessionFactory);

		}

		public ProductDto GetProduct(int productId)
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var productRepository = _dataRepositoryFactory.GetDataRepository<IProductRepository>();
					Product product = productRepository.Get(productId);
					return Mapper.Map<ProductDto>(product);
				});
		}

		public ProductDto[] GetAllProducts()
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var productRepository = _dataRepositoryFactory.GetDataRepository<IProductRepository>();
					var sentProducts = new List<ProductDto>();
					Product[] products = productRepository.GetAll();
					foreach (Product product in products)
					{
						var productDto = Mapper.Map<ProductDto>(product);
						sentProducts.Add(productDto);
					}
					return sentProducts.ToArray();
					
				});
		}

		public CategoryProductDto[] GetActiveProducts(string letter, int? page, int pageSize)
		{
			System.Console.WriteLine("InvetoryManager: {0}", this.GetHashCode());
			return ExecuteFaultHandledOperation(
				() =>
				{
					int pageIndex = page ?? 1;

					if (pageIndex < 0 || pageIndex > pageSize)
					{
						return new CategoryProductDto[] { };
					}

					int offset = (pageIndex - 1) * pageSize;
					string letterToFilter = letter ?? ""; 
					using (ISession session = _sessionFactory.OpenSession())
					{
						CategoryProductDto dtoAlias = null;
						Product keyAlias = null;
						Category categoryAlias = null;

						IList<CategoryProductDto> categoryProducts = session.QueryOver<Product>(() => keyAlias)
							.JoinAlias(p => p.Categories, () => categoryAlias, JoinType.InnerJoin)
							.Where(product => product.Archived == false)
							.WhereRestrictionOn(p => p.Name).IsInsensitiveLike(letterToFilter, MatchMode.Start)
							.OrderBy(product => product.Name).Asc
							.Select(
									Projections.Property(() => keyAlias.Name).WithAlias(() => dtoAlias.ProductName),
									Projections.Property(() => keyAlias.ProductId).WithAlias(() => dtoAlias.ProductId),
									Projections.Property(() => keyAlias.Archived).WithAlias(() => dtoAlias.Archived),
									Projections.Property(() => keyAlias.Price).WithAlias(() => dtoAlias.Price),
									Projections.Property(() => categoryAlias.Name).WithAlias(() => dtoAlias.CategoryName))
							.TransformUsing(Transformers.AliasToBean<CategoryProductDto>()).Skip(offset).Take(pageSize)
							.List<CategoryProductDto>();
						
						return categoryProducts.ToArray();
					}
				});
		}

		public int GetNumberOfActiveProducts(string letter)
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					string letterToFilter = letter ?? "";
					using (ISession session = _sessionFactory.OpenSession())
					{
						Product keyAlias = null;
						Category categoryAlias = null;

						var categoryProducts = session.QueryOver<Product>(() => keyAlias)
							.JoinAlias(p => p.Categories, () => categoryAlias, JoinType.InnerJoin)
							.Where(product => product.Archived == false)
							.WhereRestrictionOn(p => p.Name).IsInsensitiveLike(letterToFilter, MatchMode.Start);
						int rowCount = categoryProducts.RowCount();
						return rowCount;
					}
				});
		}

		[OperationBehavior(TransactionScopeRequired = true)]
		public void DeleteProduct(int productId)
		{
			ExecuteFaultHandledOperation(
				() =>
				{
					var productRepository = _dataRepositoryFactory.GetDataRepository<IProductRepository>();
					productRepository.Delete(productId);
				});
		}

		public ProductDto AddProduct(ProductDto productDto)
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var productRepository = _dataRepositoryFactory.GetDataRepository<IProductRepository>();

					Product product = productRepository.Add(Mapper.Map<Product>(productDto));
					return Mapper.Map<ProductDto>(product);
				});
		}

		public CategoryDto GetCategory(int categoryId)
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var categoryRepository = _dataRepositoryFactory.GetDataRepository<ICategoryRepository>();
					return Mapper.Map<CategoryDto>(categoryRepository.Get(categoryId));
				});
		}

		public CategoryDto[] GetAllCategories()
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var sentCategories = new List<CategoryDto>();
					var categoryRepository = _dataRepositoryFactory.GetDataRepository<ICategoryRepository>();
					Category[] categories = categoryRepository.GetAll();

					foreach (Category category in categories)
					{
						var categoryDto = Mapper.Map<CategoryDto>(category);
						sentCategories.Add(categoryDto);
					}
					return sentCategories.ToArray();
				});
		}

		[OperationBehavior(TransactionScopeRequired = true)]
		public void DeleteCategory(int categoryId)
		{
			ExecuteFaultHandledOperation(
				() =>
				{
					var categoryRepository = _dataRepositoryFactory.GetDataRepository<ICategoryRepository>();
					categoryRepository.Delete(categoryId);
				});
		}

		[OperationBehavior(TransactionScopeRequired = true)]
		public CategoryDto UpdateCategory(CategoryDto categoryDto)
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var categoryRepository = _dataRepositoryFactory.GetDataRepository<ICategoryRepository>();
					var category = Mapper.Map<Category>(categoryDto);
					categoryRepository.Update(category);
					return Mapper.Map<CategoryDto>(category);
				});
		}

		public CategoryDto AddCategory(CategoryDto categoryDto)
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var categoryRepository = _dataRepositoryFactory.GetDataRepository<ICategoryRepository>();
					var category = Mapper.Map<Category>(categoryDto);
					Category result = categoryRepository.Add(category);
					return Mapper.Map<CategoryDto>(result);
				});
		}

		

		[OperationBehavior(TransactionScopeRequired = true)]
		public ProductDto UpdateProduct(ProductDto productDto)
		{
			return ExecuteFaultHandledOperation(
				() =>
				{
					var productRepository = _dataRepositoryFactory.GetDataRepository<IProductRepository>();
					var product = Mapper.Map<Product>(productDto);
					Product result = productRepository.Update(product);
					return Mapper.Map<ProductDto>(result);
				});
		}
	}
}