using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Core.Common.Contracts;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Inventory.Data
{
	public abstract class DataRepositoryBase<T> : IDataRepository<T>
		where T : class,  IIdentifiableEntity
	{
		private readonly ISessionFactory _sessionFactory;

		public DataRepositoryBase()
		{
			
		}
			
		[ImportingConstructor]
		public DataRepositoryBase(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		/// <summary>
		///     Used to get a IQueryable that is used to retrive object from entire table.
		/// </summary>
		/// <returns>IQueryable to be used to select entities from database</returns>
		public T[] GetAll()
		{
			using (ISession session = _sessionFactory.OpenSession())
			{
				Debug.WriteLine("Session: {0}, SessionFactory= {1}", session.GetHashCode(), _sessionFactory.GetHashCode());
				IQueryable<T> queryable = session.Query<T>();
				//NHibernateUtil.Initialize(queryable.ToArray());
				return queryable.ToArray();
			}
		}


		public T[] GetAll(ICriterion[] restrictions)
		{
			using (ISession session = _sessionFactory.OpenSession())
			{
				Console.WriteLine("Session: {0}, SessionFactory= {1}", session.GetHashCode(), _sessionFactory.GetHashCode());

				ICriteria criteria = session.CreateCriteria(typeof (T));
				foreach (ICriterion criterion in restrictions)
				{
					criteria.Add(criterion);
				}

				IList<T> list = criteria.List<T>();

				return list.ToArray();
			}
		}

		/// <summary>
		///     Gets an entity.
		/// </summary>
		/// <param name="key">Primary key of the entity to get</param>
		/// <returns>Entity</returns>
		public T Get(int id)
		{
			using (ISession session = _sessionFactory.OpenSession())
			{
				return session.Get<T>(id);
			}
		}

		/// <summary>
		///     Add a new entity.
		/// </summary>
		/// <param name="entity">Entity</param>
		public T Add(T entity)
		{
			using (ISession session = _sessionFactory.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Save(entity);
				transaction.Commit();
				return entity;
			}
		}

		/// <summary>
		///     Updates an existing entity.
		/// </summary>
		/// <param name="entity">Entity</param>
		public T Update(T entity)
		{
			using (ISession session = _sessionFactory.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Update(entity);
				transaction.Commit();
				return entity;
			}
		}

		/// <summary>
		///     Deletes an entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		public void Delete(int id)
		{
			using (ISession session = _sessionFactory.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Delete(session.Load<T>(id));
				transaction.Commit();
			}
		}
	}
}