using System.ComponentModel.Composition;
using Inventory.Client.Entities;
using Inventory.Data.Contracts.Repository_Interfaces;
using NHibernate;

namespace Inventory.Data.Data_Repositories
{
	[Export(typeof (IProductRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ProductRepository : DataRepositoryBase<Product>, IProductRepository
	{
		[ImportingConstructor]
		public ProductRepository(ISessionFactory sessionFactory)
			: base(sessionFactory)
		{
		}
	}
}