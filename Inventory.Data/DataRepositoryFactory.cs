﻿using System.ComponentModel.Composition;
using Core.Common.Contracts;
using Core.Common.Core;

namespace Inventory.Data
{
	[Export(typeof (IDataRepositoryFactory))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class DataRepositoryFactory : IDataRepositoryFactory
	{
		public T GetDataRepository<T>() where T : IDataRepository
		{
			return ObjectBase.Container.GetExportedValue<T>();
		}
	}
}