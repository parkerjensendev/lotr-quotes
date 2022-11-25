using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotRQuotes.ApiServices;
using LotRQuotes.ViewModels;
using Xamarin.Forms;

namespace LotRQuotes
{
	public partial class MainPage : ContentPage
	{
		private MainPageViewModel viewModel;
		public MainPage()
		{
			viewModel = new MainPageViewModel();
			BindingContext = viewModel;
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			_ = Task.Run(viewModel.Initialize);
			base.OnAppearing();
		}
	}
}
