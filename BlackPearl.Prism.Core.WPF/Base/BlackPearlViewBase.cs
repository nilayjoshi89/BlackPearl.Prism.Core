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
            if (!(sender is BlackPearlViewBase view)
               || !(view.DataContext is BlackPearlViewModelBase viewModel))
            {
                return;
            }

            viewModel.UnloadCommandAction();
        }
        private void BlackPearlViewBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is BlackPearlViewBase view)
                || !(view.DataContext is BlackPearlViewModelBase viewModel))
            {
                return;
            }

            viewModel.LoadCommandAction();
        }
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Loaded -= BlackPearlViewBase_Loaded;
                    Unloaded -= BlackPearlViewBase_Unloaded;
                }), null);

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~BlackPearlViewBase()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
