using System;
using MyBlueSample.ViewModels;
using Xamarin.Forms;

namespace MyBlueSample.Views
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {
            BackgroundColor = Color.Black;
        }

        public object NavigationArgs { get; set; }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is BaseViewModel viewModel)
                viewModel.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is BaseViewModel baseViewModel)
                baseViewModel.OnAppearing(NavigationArgs);
        }
    }
}
