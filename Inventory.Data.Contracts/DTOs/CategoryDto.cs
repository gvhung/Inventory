using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Inventory.Data.Contracts.DTOs
{
	[DataContract(IsReference = true)]
	public class CategoryDto
	{
		private IList<ProductDto> _products;

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int CategoryId { get; set; }

		[DataMember]
		public IList<ProductDto> Products
		{
			get
			{
				if (_products == null)
				{
					_products = new List<ProductDto>();
				}
				return _products;
			}
			set { _products = value; }
		}
	}
}