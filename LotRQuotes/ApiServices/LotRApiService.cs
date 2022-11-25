using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		private static HttpClient _instance;

		private static HttpClient Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new HttpClient();
					//I would put this in config files or pull it from the server if I was doing this right.
					_instance.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", "ArKxecgybWqdt767qKpB");
				}

				return _instance;
			}
		}

		public static async Task<QuoteResponse> getQuotes(int page = 1)
		{
			Uri uri = new Uri($"https://the-one-api.dev/v2/quote?limit=25&page={page}");

			HttpResponseMessage response = await Instance.GetAsync(uri);
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

		public static async Task<Movie> getMovie(string movieId)
		{
			Uri uri = new Uri($"https://the-one-api.dev/v2/movie/{movieId}");

			HttpResponseMessage response = await Instance.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				var movieResponse = JsonConvert.DeserializeObject<MovieResponse>(responseContent);
				return movieResponse.docs.First() ?? new Movie();
			}
			else
			{
				return new Movie();
			}

		}

		public static async Task<Character> getCharacter(string characterId)
		{
			Uri uri = new Uri($"https://the-one-api.dev/v2/character/{characterId}");

			HttpResponseMessage response = await Instance.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				var characterResponse = JsonConvert.DeserializeObject<CharacterResponse>(responseContent);
				return characterResponse.docs.First() ?? new Character();
			}
			else
			{
				return new Character();
			}

		}
	}
}
