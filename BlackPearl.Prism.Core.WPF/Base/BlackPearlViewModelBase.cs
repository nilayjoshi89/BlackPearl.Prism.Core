using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Regions;

namespace BlackPearl.Prism.Core.WPF
{
    public abstract class BlackPearlViewModelBase : BindableBase, INotifyDataErrorInfo, INavigationAware, IRegionMemberLifetime, IDisposable
    {
        #region Members
        private IEnumerable<string> propertiesToValidate = null;
        private Dictionary<string, List<string>> propertyErrors = new Dictionary<string, List<string>>();
        protected IRegionNavigationService navigationService;
        private bool disposedValue;
        #endregion

        #region Constructor
        public BlackPearlViewModelBase()
        {
            InitializePropertiesToValidate();
        }
        #endregion

        #region Properties
        protected bool IsLoaded { get; private set; }
        #endregion

        #region Methods
        public async void UnloadCommandAction()
        {
            await Task.Run(async () =>
            {
                try
                {
                    await UnloadAction();
                }
                catch { }
            });
        }
        public async void LoadCommandAction()
        {
            await Task.Run(async () =>
            {
                try
                {
                    await LoadAction();
                    IsLoaded = true;
                }
                catch { }
            });
        }
        protected virtual Task LoadAction() => Task.CompletedTask;
        protected virtual Task UnloadAction() => Task.CompletedTask;
        protected virtual IEnumerable<string> GetValidationErrors(string propertyName) => null;
        protected void ClearValidationError(string propertyName)
        {
            if (disposedValue)
            {
                return;
            }

            ClearErrorWithoutNotification(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        protected void ValidateProperty(string propertyName)
        {
            if (disposedValue || string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            ClearErrorWithoutNotification(propertyName);
            IEnumerable<string> errors = null;

            try
            {
                errors = GetValidationErrors(propertyName);
            }
            catch { }

            if (errors?.Any() != true)
            {
                return;
            }

            AddError(propertyName, errors);
        }
        protected void ValidateAllProperties()
        {
            if (disposedValue)
            {
                return;
            }

            foreach (string p in propertiesToValidate)
            {
                ValidateProperty(p);
            }
        }
        protected virtual void Dispose()
        {
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            ValidateProperty(args.PropertyName);
        }
        private void AddError(string propertyName, IEnumerable<string> errors)
        {
            if (!propertyErrors.ContainsKey(propertyName))
            {
                propertyErrors.Add(propertyName, new List<string>());
            }

            List<string> currentErrorList = propertyErrors[propertyName];
            IEnumerable<string> uniqueErrors = errors.Where(e => !currentErrorList.Contains(e));
            currentErrorList.AddRange(uniqueErrors);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrorWithoutNotification(string propertyName)
        {
            if (!propertyErrors.ContainsKey(propertyName))
            {
                return;
            }

            propertyErrors[propertyName]?.Clear();
        }
        private void InitializePropertiesToValidate()
        {
            propertiesToValidate = GetType()
                                    .GetProperties()
                                    .Where(p => p.Name != nameof(HasErrors))
                                    .Select(p => p.Name);
        }
        #endregion

        #region INotifyDataErrorInfo
        public bool HasErrors => propertyErrors?.Any(pe => pe.Value?.Any() == true) == true;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            if (disposedValue)
            {
                return null;
            }

            return propertyErrors.ContainsKey(propertyName ?? string.Empty)
                    ? propertyErrors[propertyName]
                    : null;
        }
        #endregion

        #region INavigationAware
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (disposedValue)
            {
                return false;
            }

            return true;
        }
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        public virtual void OnNavigatedTo(NavigationContext navigationContext) => navigationService = navigationContext.NavigationService;
        #endregion

        #region IDisposable
        private void DisposeObject(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    propertiesToValidate = null;
                    propertyErrors?.Clear();
                    propertyErrors = null;
                    navigationService = null;
                    Dispose();
                }

                disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            DisposeObject(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IRegionMemberLifetime
        public virtual bool KeepAlive => false;
        #endregion
    }
}
