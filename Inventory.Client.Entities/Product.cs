using System.Collections.Generic;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace Inventory.Client.Entities
{
	[DataContract]
	public class Product : EntityBase, IIdentifiableEntity
	{
		[DataMember]
		public bool Archived { get; set; }

		[DataMember]
		public double Price { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int ProductId { get; set; }

		[DataMember]
		public IList<Category> Categories { get; private set; }

		public int EntityId
		{
			get { return ProductId; }
			set { ProductId = value; }
		}
	}
}