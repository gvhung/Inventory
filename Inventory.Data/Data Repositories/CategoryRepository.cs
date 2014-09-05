using System.ComponentModel.Composition;
using Inventory.Client.Entities;
using Inventory.Data.Contracts.Repository_Interfaces;
using NHibernate;

namespace Inventory.Data.Data_Repositories
{
	[Export(typeof (ICategoryRepository))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CategoryRepository : DataRepositoryBase<Category>, ICategoryRepository
	{
		[ImportingConstructor]
		public CategoryRepository(ISessionFactory sessionFactory) : base(sessionFactory)
		{
		}
	}
}