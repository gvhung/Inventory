using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Inventory.Data.Contracts.DTOs
{
	[DataContract(IsReference = true)]
	public class ProductDto
	{
		private IList<CategoryDto> _categories;

		[DataMember]
		public bool Archived { get; set; }

		[DataMember]
		public double Price { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int ProductId { get; set; }

		[DataMember]
		public IList<CategoryDto> Categories
		{
			get
			{
				if (_categories == null)
				{
					_categories = new List<CategoryDto>();
				}
				return _categories;
			}
			set { _categories = value; }
		}
	}
}