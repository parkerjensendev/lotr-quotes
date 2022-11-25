using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LotRQuotes.Models;
using Newtonsoft.Json;

namespace LotRQuotes.ApiServices
{
	//I'd probably implement a better api service here by using something like Refit if
	//if I was doing this less quickly
	public class LotRApiService
	{
		private static HttpClient _instance = new HttpClient();

		public static async Task<QuoteResponse> getQuotes()
		{
			//I would put this in config files or pull it from the server if I was doing this right.
			_instance.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", "ArKxecgybWqdt767qKpB");

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
