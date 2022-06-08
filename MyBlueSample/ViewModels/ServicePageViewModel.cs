using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyBlueSample.Interfaces;
using MyBlueSample.Services;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MyBlueSample.ViewModels
{
    public class ServicePageViewModel : BaseViewModel
    {
        IBluetooth _bluetoothService;
        INavigationService _navigationService;

        public IService Service { get; private set; }
        public ObservableCollection<ICharacteristic> Characteristics { get; private set; }
        public ICommand GoToCharacteristicCommand { get; }

        public ServicePageViewModel()
        {
            _bluetoothService = DependencyService.Resolve<IBluetooth>();
            _navigationService = DependencyService.Resolve<INavigationService>();

            RefreshCommand = new Command(async () => await LoadCharacteristics(Service), () => !IsLoading);
            GoToCharacteristicCommand = new Command<ICharacteristic>(async characteristic =>
            await _navigationService.NavigateTo<CharacteristicPageViewModel, ICharacteristic>(characteristic));
        }

        async Task LoadCharacteristics(IService service)
        {
            IsLoading = true;
            var characteristics = await _bluetoothService.GetCharacteristics(service);
            if (characteristics != null)
                Characteristics = new ObservableCollection<ICharacteristic>(characteristics);
            IsLoading = false;
        }

        public override async void OnAppearing(object args)
        {
            base.OnAppearing(args);
            if (args is IService service)
            {
                Service = service;
                await LoadCharacteristics(service);
            }
        }
    }
}
