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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is BaseViewModel viewModel)
                viewModel.OnDisappearing();
        }
    }
}
