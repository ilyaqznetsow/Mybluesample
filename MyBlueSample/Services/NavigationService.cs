using System.Threading.Tasks;
using MyBlueSample.ViewModels;
using Xamarin.Forms;

namespace MyBlueSample.Services
{
    public class NavigationService : INavigationService
    {
        public NavigationService()
        {
        }

        public void SetRoot()
        {
            var mainPage = GetPage<MainViewModel>();
            App.Current.MainPage = new NavigationPage(mainPage);
        }

        public async Task NavigateTo<TViewModel, TArgs>(TArgs args)
        {
            var page = GetPage<TViewModel>();
            await App.Current.MainPage.Navigation.PushAsync(page);
            if (page.BindingContext is BaseViewModel baseViewModel)
                baseViewModel.OnAppearing(args);
        }

        private Page GetPage<TViewModel>()
           => Routing.GetPage(typeof(TViewModel));
    }
}
