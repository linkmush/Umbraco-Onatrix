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
	public class QuestionSurfaceController : SurfaceController
	{
		public QuestionSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
		}

		public IActionResult QuestionSubmit(QuestionFormModel form)
		{
			if (!ModelState.IsValid)
			{
				ViewData["question_name"] = form.Name;
				ViewData["question_email"] = form.Email;
				ViewData["question_message"] = form.Message;

				ViewData["question_error_name"] = string.IsNullOrEmpty(form.Name);
				ViewData["question_error_email"] = string.IsNullOrEmpty(form.Email);
				ViewData["question_error_message"] = string.IsNullOrEmpty(form.Message);

				return CurrentUmbracoPage();
			}

			TempData["question_success"] = "form submitted sucessfully";
			return RedirectToCurrentUmbracoPage();
		}
	}
}
