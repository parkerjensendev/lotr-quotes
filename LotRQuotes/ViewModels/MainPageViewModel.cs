using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LotRQuotes.ApiServices;
using LotRQuotes.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LotRQuotes.ViewModels
{
	public class MainPageViewModel: INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;

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

        public ICommand SelectQuote { get; }


        public MainPageViewModel()
		{
            Quotes = new List<Quote>();
			Pages = new List<int>
			{
				1
			};
            SelectedPage = 1;
			movie = new Movie();
            SelectQuote = new Command<Quote>(async (Quote quote) => await NavToQuote(quote));

        }

		internal async Task Initialize()
		{
            Loading = true;
            var quoteResponse = await LotRApiService.getQuotes(SelectedPage);
            Quotes = quoteResponse.docs.ToList();
            var pagesRange = Enumerable.Range(1, quoteResponse.pages).ToList();
            Pages = pagesRange;
            selectedPage = 1;
            OnPropertyChanged(nameof(SelectedPage));
            Loading = false;
		}

        private async Task SelectNewPage()
		{
            Loading = true;
            var quoteResponse = await LotRApiService.getQuotes(SelectedPage);
            Quotes = quoteResponse.docs.ToList();
            Loading = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task NavToQuote(Quote quote)
		{
            Loading = true;
            SelectedQuote = quote;
            await Task.WhenAll(
                FetchMovie(quote.movie),
                FetchCharacter(quote.character)
            );

            //ShowDetails(quote, Movie, Character

            Loading = false;
		}

        public async Task FetchMovie(string movieId)
		{
            Movie = await LotRApiService.getMovie(movieId);
        }
        public async Task FetchCharacter(string characterId)
        {
            Character = await LotRApiService.getCharacter(characterId);
        }
    }
}
