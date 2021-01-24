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
            Loaded -= BlackPearlViewBase_Loaded;
            Unloaded -= BlackPearlViewBase_Unloaded;

            if (!(sender is BlackPearlViewBase view)
               || !(view.DataContext is BlackPearlViewModelBase viewModel))
            {
                return;
            }

            viewModel.UnloadCommandAction();
        }
        private void BlackPearlViewBase_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= BlackPearlViewBase_Loaded;

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

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
        }
        #endregion
    }
}
