using System.Threading;
using System.Windows.Input;
using PropertyChanged;

namespace MyBlueSample.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseViewModel
    {
        protected CancellationTokenSource TokenSource { get; private set; } = new CancellationTokenSource();
        public bool IsLoading { get; protected set; }
        public ICommand RefreshCommand { get; protected set; }

        public abstract void OnAppearing(object args);

        public void OnDisappearing()
        {
            if (TokenSource.IsCancellationRequested)
                TokenSource.Cancel();
            TokenSource?.Dispose();
        }

    }
}
