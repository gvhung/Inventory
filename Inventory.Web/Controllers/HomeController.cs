using System.ComponentModel.Composition;
using System.Web.Mvc;
using Inventory.Client.Contracts;
using Inventory.Data.Contracts.DTOs;
using Inventory.Models;

namespace Inventory.Controllers
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class HomeController : Controller
	{
		private readonly IInventoryService _inventoryService;
	
		public HomeController()
		{
		}

		[ImportingConstructor]
		public HomeController(IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
		}

		public ActionResult Index(int? page, string letter)
		{
			return
				View(new InventoryModel(
					_inventoryService.GetActiveProducts(letter, page, InventoryModel.PageSize), 
					_inventoryService.GetNumberOfActiveProducts(letter),letter
					));
		}

		public ActionResult Archive(int id)
		{
			ProductDto productDto = _inventoryService.GetProduct(id);
			productDto.Archived = true;
			_inventoryService.UpdateProduct(productDto);
			return RedirectToAction("Index");
		}


		public ActionResult About()
		{
			ViewBag.Message = "Inventory of Products and their categories";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "";

			return View();
		}
	}
}