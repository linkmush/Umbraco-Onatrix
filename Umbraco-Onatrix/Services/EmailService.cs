using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using Umbraco_Onatrix.dto;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Services;

public class EmailService(HttpClient httpClient)
{
	private readonly HttpClient _httpClient = httpClient;

	public async Task SendDataToAzureFunction(EmailDto dto)
	{
		try
		{
			// URL till din Azure Function (HTTP-trigger)
			var functionUrl = "https://emailprovider-oskar.azurewebsites.net/api/EmailConfirmation?code=qanDUy5rfbqieu0_PiNSORcjt0fmaJvGZbQf4drfVGBEAzFuzQyJ4Q%3D%3D";

			// Omvandla formulärdata till JSON
			var json = JsonConvert.SerializeObject(new
			{
				email = dto.Email
			});

			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// Skicka POST-begäran till Azure Function
			var response = await _httpClient.PostAsync(functionUrl, content);

			if (response.IsSuccessStatusCode)
			{
				Debug.WriteLine("Bekräftelsemail skickat via Azure Function.");
			}
			else
			{
				Debug.WriteLine($"Fel vid anrop till Azure Function: {response.StatusCode}");
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Fel vid anrop till Azure Function: {ex.Message}");
		}
	}
}
