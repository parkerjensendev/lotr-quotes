using System.Collections.Generic;
using System.Threading.Tasks;
using LotRQuotes.ApiServices;
using LotRQuotes.DatabaseServices;
using LotRQuotes.Models;
using LotRQuotes.ViewModels;
using NUnit.Framework;

namespace Tests
{
	public class TestApiService : ILotRApiService
	{
		public Task<Character> GetCharacter(string characterId)
		{
			throw new System.NotImplementedException();
		}

		public Task<Movie> GetMovie(string movieId)
		{
			throw new System.NotImplementedException();
		}

		public Task<QuoteResponse> GetQuotes(int page)
		{
			return Task.FromResult(new QuoteResponse()
			{

				docs = new List<Quote>()
				{
					new Quote()
					{
						_id = "1",
						dialog = "Yes"
					},
					new Quote()
					{
						_id = "2",
						dialog = "Yup"
					},
					new Quote()
					{
						_id = "3",
						dialog = "Yessir"
					},
					new Quote()
					{
						_id = "4",
						dialog = "Yeppers"
					}
				}
			});
		}
	}

	public class TestHideQuoteService : IHideQuoteService
	{
		public void AddQuoteToHide(Quote quote)
		{
			throw new System.NotImplementedException();
		}

		public List<Quote> FilterQuotes(List<Quote> quotes)
		{
			return quotes.FindAll(q => q._id != "1");
		}
	}

	public class Tests
	{
		public MainPageViewModel MainPageViewModel {get; set;}
		[SetUp]
		public void Setup()
		{
			MainPageViewModel = new MainPageViewModel(new TestApiService(), new TestHideQuoteService());
		}

		[Test]
		public async Task TestMainPageViewModel_QuotesAreFilteredUponInitialization()
		{
			Assert.IsEmpty(MainPageViewModel.Quotes);
			await MainPageViewModel.Initialize();
			Assert.IsNull(MainPageViewModel.Quotes.Find(q => q._id == "1"));
		}
	}
}
