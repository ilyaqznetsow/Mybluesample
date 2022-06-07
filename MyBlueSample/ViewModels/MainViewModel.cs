using System.Collections.ObjectModel;
using System.Windows.Input;
using MyBlueSample.Interfaces;
using MyBlueSample.Services;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MyBlueSample.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        IBluetooth _bluetoothService;
        INavigationService _navigationService;

        public ICommand PerformScanCommand { get; private set; }
        public ICommand GoToDeviceCommand { get; private set; }
        public bool IsScanning { get; private set; }

        public ObservableCollection<IDevice> DevicesCollection => _bluetoothService.DiscoveredDevices;
        public BluetoothState BluetoothState => _bluetoothService.State;

        public MainViewModel()
        {
            _bluetoothService = DependencyService.Resolve<IBluetooth>();
            _navigationService = DependencyService.Resolve<INavigationService>();

            PerformScanCommand = new Command(async () =>
            {
                if (IsScanning)
                {
                    var devices = await _bluetoothService.StopScan();
                    IsScanning = false;
                }
                else
                {
                    IsScanning = true;
                    var isStarted = await _bluetoothService.StartScan(token: TokenSource.Token);
                }
            });

            GoToDeviceCommand = new Command<IDevice>(async (device) =>
            {
                if (IsScanning)
                {
                    var devices = await _bluetoothService.StopScan();
                    IsScanning = false;
                }
                await _navigationService.NavigateTo<DevicePageViewModel, IDevice>(device);
            });
        }
    }
}

// Sample from terminal
// Connected:
//  dudos:
//      Address: 4C:B9:9B:50:26:2D
//      Vendor ID: 0x054C
//      Product ID: 0x0CE6
//      Firmware Version: 1.0.0
//      Minor Type: Gamepad
//      RSSI: -35
//      Services: 0x800020 < HID ACL >