using System;
using System.Windows;
using System.Windows.Controls;

using Prism.Mvvm;

namespace BlackPearl.Prism.Core.WPF
{
    public class BlackPearlViewBase : UserControl, IDisposable
    {
        #region Member
        private bool disposedValue;
        #endregion

        #region Constructor
        public BlackPearlViewBase()
        {
            SetValue(ViewModelLocator.AutoWireViewModelProperty, true);
            Loaded += BlackPearlViewBase_Loaded;
            Unloaded += BlackPearlViewBase_Unloaded;
        }
        #endregion

        #region Methods
        private void BlackPearlViewBase_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Unloaded -= BlackPearlViewBase_Unloaded;

                if (!(sender is BlackPearlViewBase view)
                   || !(view.DataContext is BlackPearlViewModelBase viewModel))
                {
                    return;
                }

                viewModel.OnUnload(sender, e);
            }
            catch { }
        }
        private void BlackPearlViewBase_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Loaded -= BlackPearlViewBase_Loaded;

                if (!(sender is BlackPearlViewBase view)
                    || !(view.DataContext is BlackPearlViewModelBase viewModel))
                {
                    return;
                }

                viewModel.OnLoad(sender, e);
            }
            catch { }
        }
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Loaded -= BlackPearlViewBase_Loaded;
                    Unloaded -= BlackPearlViewBase_Unloaded;
                }

                disposedValue = true;
            }
        }
        public void Dispose() => Dispose(disposing: true);
        #endregion
    }
}
