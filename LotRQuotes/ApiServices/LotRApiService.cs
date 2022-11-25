using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LotRQuotes.Models;
using Newtonsoft.Json;

namespace LotRQuotes.ApiServices
{
	public class LotRApiService
	{
		private static HttpClient _instance = new HttpClient();

		private static JsonSerializer _serializer = new JsonSerializer();

		public static async Task<QuoteResponse> getQuotes()
		{
			_instance.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", "ArKxecgybWqdt767qKpB");
			//var response = await _instance.GetAsync("/quote");

			Uri uri = new Uri(string.Format("https://the-one-api.dev/v2/quote", string.Empty));

			HttpResponseMessage response = await _instance.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				return JsonConvert.DeserializeObject<QuoteResponse>(responseContent);
			}
			else
			{
				return new QuoteResponse();
			}

		}

	}
}
