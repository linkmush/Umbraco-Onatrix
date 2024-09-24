using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Controllers
{
	public class SupportSurfaceController : SurfaceController
	{
		public SupportSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
		}

		public IActionResult SupportSubmit(SupportFormModel form)
		{
			if (!ModelState.IsValid)
			{
				ViewData["support_email"] = form.Email;

				ViewData["support_error_email"] = string.IsNullOrEmpty(form.Email);

				return CurrentUmbracoPage();
			}

			TempData["support_success"] = "form submitted sucessfully";
			return RedirectToCurrentUmbracoPage();
		}
	}
}
