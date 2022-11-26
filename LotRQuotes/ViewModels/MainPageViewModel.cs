using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LotRQuotes.ApiServices;
using LotRQuotes.Models;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LotRQuotes.ViewModels
{
	public class MainPageViewModel: INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ILotRApiService ApiService;

        private List<Quote> quotes;

        public List<Quote> Quotes
        {
            set
            {
                if (quotes != value)
                {
                    quotes = value;
                    OnPropertyChanged(nameof(Quotes));
                }
            }
            get
            {
                return quotes;
            }
        }

        private List<int> pages;

        public List<int> Pages
        {
            set
            {
                if (pages != value)
                {
                    pages = value;
                    OnPropertyChanged(nameof(Pages));
                }
            }
			get
			{
                return pages;
			}
        }

        private bool loading = true;

		public bool Loading
        {
            set
            {
                if (loading != value)
                {
                    loading = value;
                    OnPropertyChanged(nameof(Loading));
                }
            }
            get
            {
                return loading;
            }
        }

        private Movie movie;

        public Movie Movie
		{
            set
			{
                if (movie != value)
				{
                    movie = value;
                    OnPropertyChanged(nameof(Movie));
				}
			}
			get
			{
                return movie;
			}
		}

        private Character character;

        public Character Character
        {
            set
            {
                if (character != value)
                {
                    character = value;
                    OnPropertyChanged(nameof(Character));
                }
            }
            get
            {
                return character;
            }
        }

        private Quote selectedQuote;

        public Quote SelectedQuote
        {
            set
            {
                if (selectedQuote != value)
                {
                    selectedQuote = value;
                    OnPropertyChanged(nameof(SelectedQuote));
                }
            }
            get
            {
                return selectedQuote;
            }
        }

        private int selectedPage;

        public int SelectedPage
        {
            set
            {
                if (selectedPage != value)
                {
                    selectedPage = value;
                    //this is a little ugly, I should probably put this in an event listener
                    Task.Run(SelectNewPage);
                    OnPropertyChanged(nameof(SelectedPage));
                }
            }
            get
            {
                return selectedPage;
            }
        }


        public MainPageViewModel(ILotRApiService apiService)
		{
            ApiService = apiService;
            Quotes = new List<Quote>();
			Pages = new List<int>
			{
				1
			};
            SelectedPage = 1;
			movie = new Movie();

        }

		internal async Task Initialize()
		{
            Loading = true;
            //I would not use preferences to persist hidden quote date really
            //I would prefere to download the quotes to a database and add a column
			//that is isHidden. So many perks to this: database is source of truth,
            //offline capabilities, better info in the list of quotes (character image,
            //movie image, et.), syncing between devices, etc.
            if (!Preferences.ContainsKey("hiddenQuotes"))
            {
                Preferences.Set("hiddenQuotes", JsonConvert.SerializeObject(new List<string>()));
            }
            var quoteResponse = await ApiService.GetQuotes(SelectedPage);
            Quotes = quoteResponse.docs.ToList().FindAll(q => !JsonConvert.DeserializeObject<List<string>>(Preferences.Get("hiddenQuotes", "")).Contains(q._id));
            var pagesRange = Enumerable.Range(1, quoteResponse.pages).ToList();
            Pages = pagesRange;
            selectedPage = 1;
            OnPropertyChanged(nameof(SelectedPage));
            Loading = false;
		}

        private async Task SelectNewPage()
		{
            Loading = true;
            var quoteResponse = await ApiService.GetQuotes(SelectedPage);
            Quotes = quoteResponse.docs.ToList().FindAll(q => !JsonConvert.DeserializeObject<List<string>>(Preferences.Get("hiddenQuotes", "")).Contains(q._id));
            Loading = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadQuoteDetails(Quote quote)
		{
            Loading = true;
            SelectedQuote = quote;
            await Task.WhenAll(
                FetchMovie(quote.movie),
                FetchCharacter(quote.character)
            );

            Loading = false;
		}

        public void HideQuote(Quote quote)
		{
            var hiddenQutoes = JsonConvert.DeserializeObject<List<string>>(Preferences.Get("hiddenQuotes", ""));
            hiddenQutoes.Add(quote._id);
            Preferences.Set("hiddenQuotes", JsonConvert.SerializeObject(hiddenQutoes));
            //this all stems from the observable range collection not workin for me and
            //me not wanting to waste too much time figuring out why
            var quoteListClone = JsonConvert.DeserializeObject<List<Quote>>(JsonConvert.SerializeObject(quotes));
            quoteListClone.Remove(quoteListClone.Find(q => q._id == quote._id));
            Quotes = quoteListClone;
        }

        public async Task FetchMovie(string movieId)
		{
            Movie = await ApiService.GetMovie(movieId);
        }
        public async Task FetchCharacter(string characterId)
        {
            Character = await ApiService.GetCharacter(characterId);
        }
    }
}
