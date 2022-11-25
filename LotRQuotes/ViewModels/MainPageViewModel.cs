using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using LotRQuotes.ApiServices;
using LotRQuotes.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LotRQuotes.ViewModels
{
	public class MainPageViewModel: INotifyPropertyChanged
	{
		public ObservableRangeCollection<Quote> Quotes { get; set; }

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


        public MainPageViewModel()
		{
            Quotes = new ObservableRangeCollection<Quote>();
		}

		internal async Task Initialize()
		{
            Loading = true;
            var quoteResponse = await LotRApiService.getQuotes();
            Quotes.AddRange(quoteResponse.docs);
           // Loading = false;
		}

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
