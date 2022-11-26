using System.Threading.Tasks;
using LotRQuotes.ApiServices;
using LotRQuotes.Models;
using LotRQuotes.ViewModels;
using Xamarin.Forms;

namespace LotRQuotes
{
	public partial class MainPage : ContentPage
	{
		private readonly MainPageViewModel viewModel;
		public MainPage()
		{
			viewModel = new MainPageViewModel(new LotRApiService());
			BindingContext = viewModel;
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			_ = Task.Run(viewModel.Initialize);
			base.OnAppearing();
		}

		//I'd prefer this as a command handled by the view model navigating to a modal popup
		//Also bad with the async void
		async void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var quote = e.Item as Quote;
			await viewModel.LoadQuoteDetails(quote);

			bool justClose = await DisplayAlert(viewModel.Movie.name, $"\"{quote.dialog}\" - {viewModel.Character.name}", "Done", "Hide");
			if (!justClose)
			{
				viewModel.HideQuote(quote);
			}
		}

		void MenuItem_Hide(System.Object sender, System.EventArgs e)
		{
			var item = sender as MenuItem;
			var quote = item.CommandParameter as Quote;
			viewModel.HideQuote(quote);
		}
	}
}
