using System.Runtime.Serialization;
using System.ServiceModel;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Inventory.Data.Contracts.DTOs;

namespace Inventory.Client.Contracts
{
	[ServiceContract]
	public interface IInventoryService : IServiceContract
	{
		[OperationContract]
		[FaultContract(typeof (NotFoundException))]
		ProductDto GetProduct(int productId);

		[OperationContract]
		ProductDto[] GetAllProducts();

		[OperationContract]
		CategoryProductDto[] GetActiveProducts(string letter, int? page, int pageSize);

		[OperationContract]
		[TransactionFlow(TransactionFlowOption.Allowed)]
		void DeleteProduct(int productId);

		[OperationContract]
		[TransactionFlow(TransactionFlowOption.Allowed)]
		ProductDto UpdateProduct(ProductDto product);

		[OperationContract]
		[TransactionFlow(TransactionFlowOption.Allowed)]
		ProductDto AddProduct(ProductDto product);

		[OperationContract]
		[FaultContract(typeof (NotFoundException))]
		CategoryDto GetCategory(int categoryId);

		[OperationContract]
		CategoryDto[] GetAllCategories();

		[OperationContract]
		[TransactionFlow(TransactionFlowOption.Allowed)]
		void DeleteCategory(int categoryId);

		[OperationContract]
		[TransactionFlow(TransactionFlowOption.Allowed)]
		CategoryDto UpdateCategory(CategoryDto category);

		[OperationContract]
		[TransactionFlow(TransactionFlowOption.Allowed)]
		CategoryDto AddCategory(CategoryDto category);

		[OperationContract]
		int GetNumberOfActiveProducts(string letter);
	}
}