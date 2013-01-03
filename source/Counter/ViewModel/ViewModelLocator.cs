/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Counter"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;

namespace CountSmart.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static MainViewModel _main;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time services and viewmodels
            ////}
            ////else
            ////{
            ////    // Create run time services and view models
            ////}

            _main = new MainViewModel();
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return _main;
            }
        }

        private static CounterViewModel _counterViewModel;

        /// <summary>
        /// Gets the CounterViewModel property.
        /// </summary>
        public static CounterViewModel CounterViewModelStatic
        {
            get
            {
                if (_counterViewModel == null)
                {
                    CreateCounterViewModel();
                }

                return _counterViewModel;
            }
        }

        /// <summary>
        /// Gets the CounterViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public CounterViewModel CounterViewModel
        {
            get
            {
                return CounterViewModelStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the CounterViewModel property.
        /// </summary>
        public static void ClearCounterViewModel()
        {
            _counterViewModel.Cleanup();
            _counterViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the CounterViewModel property.
        /// </summary>
        public static void CreateCounterViewModel()
        {
            if (_counterViewModel == null)
            {
                _counterViewModel = new CounterViewModel();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearCounterViewModel();
        }
    }
}