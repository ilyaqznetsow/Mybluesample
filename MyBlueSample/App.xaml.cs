using MyBlueSample.Interfaces;
using MyBlueSample.Services;
using MyBlueSample.ViewModels;
using MyBlueSample.Views;
using Xamarin.Forms;

namespace MyBlueSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<IBluetooth, BluetoothWorker>();
            DependencyService.Register<INavigationService, NavigationService>();

            Routing.RegisterRoute(typeof(MainViewModel), typeof(MainPage));
            Routing.RegisterRoute(typeof(DevicePageViewModel), typeof(DevicePage));
            Routing.RegisterRoute(typeof(CharacteristicPageViewModel), typeof(CharacteristicPage));
            Routing.RegisterRoute(typeof(ServicePageViewModel), typeof(ServicePage));
            Routing.RegisterRoute(typeof(CustomAnimationPageViewModel), typeof(CustomAnimationPage));

            DependencyService.Resolve<INavigationService>().SetRoot();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
