using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;

namespace Inventory.Business.Entities
{
	[DataContract]
	public class Category: EntityBase, IIdentifiableEntity
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
