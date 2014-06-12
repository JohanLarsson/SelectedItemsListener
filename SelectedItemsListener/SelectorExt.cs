namespace SelectedItemsListener
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public static class SelectorExt
    {
        public static readonly DependencyProperty SelectedItemsBindableProperty = DependencyProperty.RegisterAttached(
            "SelectedItemsBindable",
            typeof(IList),
            typeof(SelectorExt),
            new PropertyMetadata(default(IList), OnSelectedItemsChanged));

        public static void SetSelectedItemsBindable(Selector element, IList value)
        {
            element.SetValue(SelectedItemsBindableProperty, value);
        }

        public static IList GetSelectedItemsBindable(Selector element)
        {
            return (IList)element.GetValue(SelectedItemsBindableProperty);
        }

        private static void OnSelectedItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var listBox = o as ListBox;
            INotifyCollectionChanged source = null;
            if (listBox != null)
            {
                source = (INotifyCollectionChanged)listBox.SelectedItems;
            }
            else
            {
                var multiSelector = o as MultiSelector;
                if (multiSelector != null)
                {
                    source = (INotifyCollectionChanged)multiSelector.SelectedItems;
                }
            }
            if (source == null)
            {
                throw new ArgumentException("Did not find property SelectedItems on " + o.GetType().Name, "o");
            }
            if (e.OldValue != null)
            {
                CollectionChangedEventManager.RemoveHandler(source, (sender, args) => Update((IList)sender, args, (IList)e.OldValue));
            }
            if (e.NewValue != null)
            {
                CollectionChangedEventManager.AddHandler(source, (sender, args) => Update((IList)sender, args, (IList)e.NewValue));
                Update((IList)source, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), (IList)e.NewValue);
            }
        }

        private static void Update(IList source, NotifyCollectionChangedEventArgs args, IList target)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < args.NewItems.Count; i++)
                    {
                        var item = args.NewItems[i];
                        target.Insert(args.NewStartingIndex + i, item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < args.OldItems.Count; i++)
                    {
                        target.RemoveAt(args.OldStartingIndex + i);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Not sure replace can happen");
                    break;
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException("Not sure move can happen");
                    break;
                case NotifyCollectionChangedAction.Reset:
                    target.Clear();
                    foreach (var item in source)
                    {
                        target.Add(item);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}