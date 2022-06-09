using System.Threading.Tasks;
using MyBlueSample.ViewModels;
using MyBlueSample.Views;
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
            Application.Current.MainPage = new NavigationPage(mainPage) { BarBackground = Color.Black, BarTextColor = Color.White };
        }

        public async Task NavigateTo<TViewModel>(object args)
        {
            var page = GetPage<TViewModel>();
            if (page is BasePage basePage)
                basePage.NavigationArgs = args;
            await Application.Current.MainPage.Navigation.PushAsync(page);

        }

        private Page GetPage<TViewModel>()
           => Routing.GetPage(typeof(TViewModel));
    }
}
