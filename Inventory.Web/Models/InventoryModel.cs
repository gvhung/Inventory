using System.Collections.Generic;
using Inventory.Data.Contracts.DTOs;

namespace Inventory.Models
{
	public class InventoryModel
	{
		private readonly IEnumerable<CategoryProductDto> _categoryProducts;
		private int _totalNumberOfEntries = 0;
		public const int PageSize = 10;

		public InventoryModel(IEnumerable<CategoryProductDto> products,int totalNumberOfEntries, string filterLetter)
		{
			_categoryProducts = products;
			_totalNumberOfEntries = totalNumberOfEntries;
			FilterLetter = filterLetter;
		}

		public IEnumerable<CategoryProductDto> CategoryProducts
		{
			get { return _categoryProducts; }
		}

		public int TotalRows
		{
			get { return _totalNumberOfEntries; }
		}

		public string FilterLetter { get; set; }
	}
}