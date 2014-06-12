namespace SelectedItemsListener
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class Vm : INotifyPropertyChanged
    {
        private readonly ObservableCollection<object> _selectedItems = new ObservableCollection<object>();
        private readonly ObservableCollection<DummyClass> _selectedClasses = new ObservableCollection<DummyClass>();
        private object _highlightedItem;
        private object _selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<object> SelectedItems
        {
            get { return _selectedItems; }
        }
        public ObservableCollection<DummyClass> SelectedClasses
        {
            get { return _selectedClasses; }
        }

        public IEnumerable<int> Values
        {
            get
            {
                return new[] { 1, 2, 3, 4, 5 };
            }
        }

        public IEnumerable<DummyClass> ClassValues
        {
            get
            {
                return Enumerable.Range(1, 5)
                                 .Select(i => new DummyClass(i));
            }
        } 

        public object HighlightedItem
        {
            get
            {
                return _highlightedItem;
            }
            set
            {
                if (Equals(value, _highlightedItem))
                {
                    return;
                }
                _highlightedItem = value;
                OnPropertyChanged();
            }
        }
        
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (Equals(value, _selectedItem))
                {
                    return;
                }
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}