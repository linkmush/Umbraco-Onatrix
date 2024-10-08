﻿using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco_Onatrix.Models;
using System.Diagnostics;
using Umbraco_Onatrix.Services;
using Umbraco_Onatrix.dto;

namespace Umbraco_Onatrix.Controllers
{
	public class ContactSurfaceController : SurfaceController
	{
		private readonly IScopeProvider _scopeProvider;
		private readonly EmailService _emailService;

		public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IScopeProvider scopeProvider , EmailService emailService, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_scopeProvider = scopeProvider;
			_emailService = emailService;
		}

		[HttpPost]
		public async Task<IActionResult> HandleSubmit(ContactFormModel form, EmailDto dto)
		{
			if (!ModelState.IsValid) 
			{
				ViewData["name"] = form.Name;
				ViewData["email"] = form.Email;
				ViewData["phone"] = form.Phone;

				ViewData["error_name"] = string.IsNullOrEmpty(form.Name);
				ViewData["error_email"] = string.IsNullOrEmpty(form.Email);
				ViewData["error_phone"] = string.IsNullOrEmpty(form.Phone);

				return CurrentUmbracoPage();
			}

			try
			{
				using (var scope = _scopeProvider.CreateScope())
				{
					var db = scope.Database;

					var sql = "INSERT INTO CustomerForm (Name, Email, Phone, SelectedArea) VALUES (@Name, @Email, @Phone, @SelectedArea)";

					var parameters = new
					{
						Name = form.Name,
						Email = form.Email,
						Phone = form.Phone,
						SelectedArea = form.SelectedArea,
					};

					db.Execute(sql, parameters);

					scope.Complete();
				}

				await _emailService.SendDataToAzureFunction(dto);
			}
			catch (Exception ex) 
			{
				Debug.WriteLine(ex.Message);
			}

			TempData["success"] = "form submitted sucessfully";
			return RedirectToCurrentUmbracoPage();
		}
	}
}
