using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

namespace MyBlueSample.Interfaces
{
    public class BluetoothWorker : IBluetooth
    {
        public bool IsAvailable => CrossBluetoothLE.Current.IsAvailable;
        public bool IsOn => CrossBluetoothLE.Current.IsOn;
        public bool IsScanning { get; private set; }
        public BluetoothState State { get; private set; }
        public IReadOnlyList<IDevice> ConnectedDevices => CrossBluetoothLE.Current.Adapter.ConnectedDevices;

        public ObservableCollection<IDevice> DiscoveredDevices { get; private set; }

        public BluetoothWorker()
        {
            //init state
            State = CrossBluetoothLE.Current.State;
            CrossBluetoothLE.Current.StateChanged += Current_StateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
            DiscoveredDevices = new ObservableCollection<IDevice>();
        }

        private void Adapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (!DiscoveredDevices.Contains(e.Device)) DiscoveredDevices.Add(e.Device);

            Debug.WriteLine($"discovered {e.Device.Id} {e.Device.Name} rssi: {e.Device.Rssi}");
        }

        private void Current_StateChanged(object sender, Plugin.BLE.Abstractions.EventArgs.BluetoothStateChangedArgs e)
        {
            State = e.NewState;
            Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
        }

        public async Task<bool> StartScan(ScanFilterOptions options = null,
            TimeSpan? timeOut = null, ScanMode mode = ScanMode.LowLatency, CancellationToken? token = null)
        {
            try
            {
                if (timeOut == null)
                    timeOut = TimeSpan.FromSeconds(int.MaxValue);
                DiscoveredDevices.Clear();
                CrossBluetoothLE.Current.Adapter.ScanTimeout = (int)timeOut.Value.TotalSeconds;
                CrossBluetoothLE.Current.Adapter.ScanMode = mode;
                await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync(cancellationToken: token ?? default);
                IsScanning = true;
                return IsScanning;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                IsScanning = false;
                return false;
            }
        }

        public async Task<IReadOnlyList<IDevice>> StopScan()
        {
            try
            {
                await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
                //возвращаем список для отладки и сравнения
                return CrossBluetoothLE.Current.Adapter.DiscoveredDevices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
            finally
            {
                IsScanning = false;
            }
        }


        public async Task<IDevice> ConnectTo(IDevice device, ConnectParameters parameters = default, CancellationToken? token = null)
        {
            try
            {
                await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(device, parameters, token ?? default);
                return ConnectedDevices.FirstOrDefault(connected => connected.Id == device.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> Disconnect(IDevice device)
        {
            try
            {
                await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(device);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<IReadOnlyList<IService>> GetServices(IDevice device, CancellationToken? token = null)
        {
            try
            {
                return await device.GetServicesAsync(token ?? default);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IService> GetService(IDevice device, Guid id, CancellationToken? token = null)
        {
            try
            {
                return await device.GetServiceAsync(id, token ?? default);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IReadOnlyList<ICharacteristic>> GetCharacteristics(IService service)
        {
            try
            {
                return await service.GetCharacteristicsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IReadOnlyList<IDescriptor>> GetDescriptors(ICharacteristic characteristic, CancellationToken? token = null)
        {
            try
            {
                return await characteristic.GetDescriptorsAsync(token ?? default);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}

