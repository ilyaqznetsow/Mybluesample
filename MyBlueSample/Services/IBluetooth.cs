using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

namespace MyBlueSample.Interfaces
{
    public interface IBluetooth
    {
        bool IsAvailable { get; }
        bool IsOn { get; }
        bool IsScanning { get; }
        BluetoothState State { get; }
        IReadOnlyList<IDevice> ConnectedDevices { get; }
        ObservableCollection<IDevice> DiscoveredDevices { get; }
        Task<bool> StartScan(ScanFilterOptions options = null,
           TimeSpan? timeOut = null, ScanMode mode = ScanMode.Balanced, CancellationToken? token = null);
        Task<IReadOnlyList<IDevice>> StopScan();
        Task<IDevice> ConnectTo(IDevice device, ConnectParameters parameters = default, CancellationToken? token = null);
        Task<bool> Disconnect(IDevice device);
        Task<IReadOnlyList<IService>> GetServices(IDevice device, CancellationToken? token = null);
        Task<IService> GetService(IDevice device, Guid id, CancellationToken? token = null);
        Task<IReadOnlyList<ICharacteristic>> GetCharacteristics(IService service);
        Task<IReadOnlyList<IDescriptor>> GetDescriptors(ICharacteristic characteristic, CancellationToken? token = null);
    }
}

