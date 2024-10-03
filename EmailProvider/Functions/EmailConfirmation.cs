using Azure.Communication.Email;
using Azure;
using EmailProvider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EmailProvider.Functions;

public class EmailConfirmation(EmailClient emailClient)
{
	private readonly EmailClient _emailClient = emailClient;

	[Function(nameof(EmailConfirmation))]
	public async Task<IActionResult> Run(
		[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
	{
		string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
		dynamic data = JsonConvert.DeserializeObject(requestBody)!;
		string email = data!.email;

		if (!string.IsNullOrEmpty(email))
		{
			var subject = "Conformation";

			var emailRequest = new EmailRequest()
			{
				To = email,
				Subject = subject,
				HtmlBody = @"
				<html>
					<body>
						<h1>Thank you for contacting us!</h1>
						<p>We have received your request and will contact you shortly.</p>
						<p>Best Regards,<br>Onatrix</p>
						<footer>
							<p>This is an automatically generated message. Please do not reply to this message.</p>
						</footer>
					</body>
				</html>",
				PlainText = "Thank you for contacting us"
			};

			bool emailSent = await SendEmailAsync(emailRequest);

			if (emailSent)
			{
				return new OkObjectResult($"ConformationEmail send to {email}");
			}
			else
			{
				return new BadRequestObjectResult("Email could not be sent.");
			}
		}
		return new BadRequestObjectResult("Please pass a valid email.");
	}

	public async Task<bool> SendEmailAsync(EmailRequest emailRequest)
	{
		try
		{
			var result = await _emailClient.SendAsync(
				WaitUntil.Completed,
				senderAddress: Environment.GetEnvironmentVariable("SenderAddress"),
				recipientAddress: emailRequest.To,
				subject: emailRequest.Subject,
				htmlContent: emailRequest.HtmlBody,
				plainTextContent: emailRequest.PlainText);

			return result.HasCompleted;
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			return false;
		}
	}
}
