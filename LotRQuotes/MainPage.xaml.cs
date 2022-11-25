using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotRQuotes.ApiServices;
using Xamarin.Forms;

namespace LotRQuotes
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			Task.Run(GetQuotes);
		}

		private async Task GetQuotes()
		{
			var quotes = await LotRApiService.getQuotes();
			System.Diagnostics.Debug.WriteLine($"QUOTES: {quotes.docs.Count}");
		}
	}
}
