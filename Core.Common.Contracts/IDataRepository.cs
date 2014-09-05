using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace Core.Common.Contracts
{
	public interface IDataRepository
	{
	}

	public interface IDataRepository<T> : IDataRepository
		where T : class
	{
		T[] GetAll();
		T[] GetAll(ICriterion[] restrictions);
		T Get(int id);
		T Add(T entity);
		T Update(T entity);
		void Delete(int id);
	}
}