using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Core.Common.Core;
using Inventory.Client.Bootstrapper;
using Inventory.Controllers;

namespace Inventory
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			var catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));


			CompositionContainer container = MEFLoader.Init(catalog.Catalogs);
			ObjectBase.Container = container;
			IControllerFactory mefControllerFactory = new MefControllerFactory(container);
			ControllerBuilder.Current.SetControllerFactory(mefControllerFactory);
		}
	}
}