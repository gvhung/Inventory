using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Contracts.DTOs
{
	[DataContract(IsReference = true)]
	public class CategoryProductDto
	{
		[DataMember]
		public bool Archived { get; set; }

		[DataMember]
		public double Price { get; set; }

		[DataMember]
		public string ProductName { get; set; }

		[DataMember]
		public string CategoryName { get; set; }

		[DataMember]
		public int ProductId { get; set; }

		
	}
}
