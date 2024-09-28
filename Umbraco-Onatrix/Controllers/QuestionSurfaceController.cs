﻿using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco_Onatrix.Models;
using Umbraco.Cms.Infrastructure.Scoping;
using System.Diagnostics;

namespace Umbraco_Onatrix.Controllers
{
	public class QuestionSurfaceController : SurfaceController
	{
		private readonly IScopeProvider _scopeProvider;

		public QuestionSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IScopeProvider scopeProvider, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_scopeProvider = scopeProvider;
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

			try
			{
				using (var scope = _scopeProvider.CreateScope())
				{
					var db = scope.Database;

					var sql = "INSERT INTO QuestionForm (Name, Email, Message) VALUES (@Name, @Email, @Message)";

					var parameters = new
					{
						Name = form.Name,
						Email = form.Email,
						Message = form.Message
					};

					db.Execute(sql, parameters);

					scope.Complete();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

			TempData["question_success"] = "form submitted sucessfully";
			return RedirectToCurrentUmbracoPage();
		}
	}
}
