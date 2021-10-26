using System;
using System.Collections.Specialized;

using Prism.Common;
using Prism.Regions;

namespace BlackPearl.Prism.Core.WPF.Region
{
    public class DisposeClosedViewsBehavior : RegionBehavior
    {
        protected override void OnAttach() =>
            Region.Views.CollectionChanged += Views_CollectionChanged;

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove
                && e.Action != NotifyCollectionChangedAction.Replace)
            {
                return;
            }

            foreach (object removedView in e.OldItems)
            {
                MvvmHelpers.ViewAndViewModelAction<IDisposable>(removedView, d =>
                {
                    try
                    {
                        d.Dispose();
                    }
                    catch { }
                });
            }
        }
    }
}
