using System;
using System.Threading.Tasks;

namespace MyBlueSample.Services
{
    public interface INavigationService
    {
        void SetRoot();
        Task NavigateTo<TViewModel, TArgs>(TArgs args);
    }
}
