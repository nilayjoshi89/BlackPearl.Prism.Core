namespace BlackPearl.Prism.Core.WPF
{
    public class PropertyChangeArg<T>
    {
        public PropertyChangeArg(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue { get; set; }
        public T NewValue { get; set; }
    }
}
