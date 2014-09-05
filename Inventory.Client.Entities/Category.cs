using System.Collections.Generic;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace Inventory.Client.Entities
{
	[DataContract]
	public class Category : EntityBase, IIdentifiableEntity
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int CategoryId { get; set; }

		[DataMember]
		public IList<Product> Products { get; private set; }


		public int EntityId
		{
			get { return CategoryId; }
			set { CategoryId = value; }
		}
	}
}