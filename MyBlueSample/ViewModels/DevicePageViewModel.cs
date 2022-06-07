using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyBlueSample.Interfaces;
using MyBlueSample.Services;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MyBlueSample.ViewModels
{
    public class DevicePageViewModel : BaseViewModel
    {
        IBluetooth _bluetoothService;
        INavigationService _navigationService;

        public IDevice Device { get; private set; }
        public ObservableCollection<IService> Services { get; private set; }
        public ICommand GoToServiceCommand { get; }

        public DevicePageViewModel()
        {
            _bluetoothService = DependencyService.Resolve<IBluetooth>();
            _navigationService = DependencyService.Resolve<INavigationService>();
            RefreshCommand = new Command(async () => await LoadServices(Device), () => !IsLoading);

            GoToServiceCommand = new Command<IService>(async service =>
            await _navigationService.NavigateTo<ServicePageViewModel, IService>(service));
        }

        public override async void OnAppearing(object args)
        {
            if (args is IDevice device)
            {
                Device = device;
                if (device.State == Plugin.BLE.Abstractions.DeviceState.Disconnected)
                    await ConnectToDevice(device);
                await LoadServices(device);
            }
        }

        async Task LoadServices(IDevice device)
        {
            IsLoading = true;
            var services = await _bluetoothService.GetServices(device, TokenSource.Token);
            Services = new ObservableCollection<IService>(services);
            IsLoading = false;
        }

        async Task ConnectToDevice(IDevice device)
        {
            var updatedState = await _bluetoothService.ConnectTo(device, token: TokenSource.Token);
            Device = updatedState;
        }
    }
}
