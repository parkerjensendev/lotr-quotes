using System;
using System.Collections.Generic;
using LotRQuotes.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace LotRQuotes.DatabaseServices
{
	//I would not use preferences to persist hidden quote date really
	//I would prefere to download the quotes to a database and add a column
	//that is isHidden. So many perks to this: database is source of truth,
	//offline capabilities, better info in the list of quotes (character image,
	//movie image, et.), syncing between devices, etc.
	public interface IHideQuoteService
	{
		public List<Quote> FilterQuotes(List<Quote> quotes);
		public void AddQuoteToHide(Quote quote);
	}
	public class HideQuoteService: IHideQuoteService
	{
		public HideQuoteService()
		{
			if (!Preferences.ContainsKey("hiddenQuotes"))
			{
				Preferences.Set("hiddenQuotes", JsonConvert.SerializeObject(new List<string>()));
			}
		}

		public void AddQuoteToHide(Quote quote)
		{
			var hiddenQutoes = JsonConvert.DeserializeObject<List<string>>(Preferences.Get("hiddenQuotes", ""));
			hiddenQutoes.Add(quote._id);
			Preferences.Set("hiddenQuotes", JsonConvert.SerializeObject(hiddenQutoes));
		}

		public List<Quote> FilterQuotes(List<Quote> quotes)
		{
			return quotes.FindAll(q => !JsonConvert.DeserializeObject<List<string>>(Preferences.Get("hiddenQuotes", "")).Contains(q._id));
		}
	}
}
