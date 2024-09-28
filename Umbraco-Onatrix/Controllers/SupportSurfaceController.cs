using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Controllers
{
	public class SupportSurfaceController : SurfaceController
	{
		private readonly IScopeProvider _scopeProvider;
		public SupportSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IScopeProvider scopeProvider, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_scopeProvider = scopeProvider;
		}

		public IActionResult SupportSubmit(SupportFormModel form)
		{
			if (!ModelState.IsValid)
			{
				ViewData["support_email"] = form.Email;

				ViewData["support_error_email"] = string.IsNullOrEmpty(form.Email);

				return CurrentUmbracoPage();
			}

			try
			{
				using (var scope = _scopeProvider.CreateScope())
				{
					var db = scope.Database;

					var sql = "INSERT INTO SupportForm (Email) VALUES (@Email)";

					var parameters = new
					{
						Email = form.Email
					};

					db.Execute(sql, parameters);

					scope.Complete();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

			TempData["support_success"] = "form submitted sucessfully";
			return RedirectToCurrentUmbracoPage();
		}
	}
}
