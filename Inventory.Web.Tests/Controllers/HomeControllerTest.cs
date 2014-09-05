using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inventory.Client.Contracts;
using Inventory.Controllers;
using Inventory.Data.Contracts.DTOs;
using Inventory.Models;
using NSubstitute;
using NUnit.Framework;

namespace Inventory.Tests.Controllers
{
	[TestFixture]
	public class HomeControllerTest
	{
		[Test]
		public void About()
		{
			var mockedInventoryService = Substitute.For<IInventoryService>();
			// Arrange
			var controller = new HomeController(mockedInventoryService);

			// Act
			var result = controller.About() as ViewResult;

			// Assert
			Assert.AreEqual("Inventory of Products and their categories", result.ViewBag.Message);
		}

		[Test]
		public void Contact()
		{
			var mockedInventoryService = Substitute.For<IInventoryService>();
			// Arrange
			var controller = new HomeController(mockedInventoryService);

			// Act
			var result = controller.Contact() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[Test]
		public void Index()
		{
			string letter = "C";

			var mockedInventoryService = Substitute.For<IInventoryService>();
			// Arrange
			var controller = new HomeController(mockedInventoryService);

			var products = new List<CategoryProductDto>
			               {
				               new CategoryProductDto
				               {
					               ProductId = 1, ProductName = "Carpet", Price = 33.4,
								   CategoryName = "Home"
				               },
				               new CategoryProductDto
				               {
					               ProductId = 2, ProductName =  "Chair",
								   CategoryName = "Home"
				               },
							    new CategoryProductDto
				               {
					               ProductId = 3, ProductName =  "Desk",
								   CategoryName = "Home"
				               }
			               };
			CategoryProductDto[] categoryProductDtos = products.Where(dto => dto.ProductName.StartsWith(letter)).ToArray();
			mockedInventoryService.GetActiveProducts(letter, 0, 10).Returns(categoryProductDtos);
			mockedInventoryService.GetNumberOfActiveProducts(letter).Returns(categoryProductDtos.Length);

			// Act
			var result = controller.Index(0,letter) as ViewResult;
			InventoryModel model = (InventoryModel)result.Model;

			

			// Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(model.CategoryProducts.Count()== products.Count);
			Assert.IsTrue(model.CategoryProducts.ElementAt(0).ProductName == products[0].ProductName);
			Assert.IsTrue(model.CategoryProducts.ElementAt(1).ProductName == products[1].ProductName);
		}
	}
}