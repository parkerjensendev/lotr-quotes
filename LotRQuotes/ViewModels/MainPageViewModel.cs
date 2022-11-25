using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
		public ObservableRangeCollection<Quote> Quotes { get; set; }

        public ICommand SelectQuote { get; }

		private bool loading = true;

		public event PropertyChangedEventHandler PropertyChanged;

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


        public MainPageViewModel()
		{
            Quotes = new ObservableRangeCollection<Quote>();
            movie = new Movie();
            SelectQuote = new Command<Quote>(async (Quote quote) => await NavToQuote(quote));

        }

		internal async Task Initialize()
		{
            Loading = true;
            var quoteResponse = await LotRApiService.getQuotes();
            Quotes.AddRange(quoteResponse.docs);
            Loading = false;
		}

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task NavToQuote(Quote quote)
		{
            Loading = true;
            Movie = await LotRApiService.getMovie(quote.movie);
            Loading = false;
		}
    }
}
