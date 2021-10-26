using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Prism.Mvvm;
using Prism.Regions;

namespace BlackPearl.Prism.Core.WPF
{
    public abstract class BlackPearlViewModelBase : BindableBase, INotifyDataErrorInfo, INavigationAware, IRegionMemberLifetime, IDisposable
    {
        #region Members
        private ConcurrentDictionary<string, List<string>> propertyErrors = new ConcurrentDictionary<string, List<string>>();
        protected IRegionNavigationService navigationService;
        private bool isDisposed;
        #endregion

        #region Constructor
        public BlackPearlViewModelBase()
        {
            Initialize();
        }
        #endregion

        #region Methods

        public Task OnLoad(object sender, RoutedEventArgs e) => Task.CompletedTask;
        public Task OnUnload(object sender, RoutedEventArgs e) => Task.CompletedTask;

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (isDisposed)
            {
                return;
            }

            base.OnPropertyChanged(args);
            ValidatePropertyInternal(args?.PropertyName);
        }

        protected void ClearValidationError(string propertyName)
        {
            if (isDisposed)
            {
                return;
            }

            ClearValidationErrorWithoutNotification(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        protected void ClearValidationErrorWithoutNotification(string propertyName)
        {
            if (isDisposed || !propertyErrors.ContainsKey(propertyName))
            {
                return;
            }

            propertyErrors[propertyName]?.Clear();
        }

        protected virtual void Initialize() { }
        protected virtual IEnumerable<string> ValidateProperty(string propertyName) { yield break; }
        protected virtual void Dispose() { }

        private void AddError(string propertyName, IEnumerable<string> errors)
        {
            propertyErrors.AddOrUpdate(propertyName, new List<string>(errors), (k, v) => UpdateConcurrentDictionaryFactory(k, v, errors));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ValidatePropertyInternal(string propertyName)
        {
            try
            {
                if (isDisposed || string.IsNullOrEmpty(propertyName))
                {
                    return;
                }

                ClearValidationErrorWithoutNotification(propertyName);
                IEnumerable<string> errors = ValidateProperty(propertyName);

                if (errors?.Any() != true)
                {
                    return;
                }

                AddError(propertyName, errors);
            }
            catch { }
        }
        private static List<string> UpdateConcurrentDictionaryFactory(string key, List<string> existingList, IEnumerable<string> newValues)
        {
            existingList.AddRange(newValues);
            return existingList.Distinct().ToList();
        }
        #endregion

        #region INotifyDataErrorInfo
        public bool HasErrors => propertyErrors?.Any(pe => pe.Value?.Any() == true) == true;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            return isDisposed
                    ? null
                    : (IEnumerable)(propertyErrors.ContainsKey(propertyName ?? string.Empty)
                        ? propertyErrors[propertyName]
                        : null);
        }
        #endregion

        #region INavigationAware
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => !isDisposed;
        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }
        public virtual void OnNavigatedTo(NavigationContext navigationContext) => navigationService = navigationContext.NavigationService;
        #endregion

        #region IDisposable
        private void DisposeObject(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    propertyErrors?.Clear();
                    propertyErrors = null;
                    navigationService = null;
                    Dispose();
                }

                isDisposed = true;
            }
        }
        void IDisposable.Dispose()
        {
            DisposeObject(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IRegionMemberLifetime
        public virtual bool KeepAlive => true;
        #endregion
    }
}
