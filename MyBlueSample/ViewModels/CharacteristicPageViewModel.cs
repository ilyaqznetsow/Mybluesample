using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyBlueSample.Interfaces;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace MyBlueSample.ViewModels
{
    public class CharacteristicPageViewModel : BaseViewModel
    {
        IBluetooth _bluetoothService;

        public ICharacteristic Characteristic { get; private set; }
        public ObservableCollection<IDescriptor> Descriptors { get; private set; }

        public CharacteristicPageViewModel()
        {
            _bluetoothService = DependencyService.Resolve<IBluetooth>();
            RefreshCommand = new Command(async () => await LoadDescriptors(Characteristic), () => !IsLoading);
        }

        async Task LoadDescriptors(ICharacteristic characteristic)
        {
            IsLoading = true;
            var descriptors = await _bluetoothService.GetDescriptors(characteristic, TokenSource.Token);
            if (descriptors != null)
                Descriptors = new ObservableCollection<IDescriptor>(descriptors);
            IsLoading = false;
        }

        public override async void OnAppearing(object args)
        {
            base.OnAppearing(args);
            if (args is ICharacteristic characteristic)
            {
                Characteristic = characteristic;
                await LoadDescriptors(characteristic);
            }
        }
    }
}
