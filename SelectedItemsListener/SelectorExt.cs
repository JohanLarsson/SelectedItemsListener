namespace SelectedItemsListener
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;

    public static class SelectorExt
    {
        public static readonly DependencyProperty SelectedItemsBindableProperty = DependencyProperty.RegisterAttached(
            "SelectedItemsBindable",
            typeof(IList),
            typeof(SelectorExt),
            new PropertyMetadata(default(IList), OnSelectedItemsChanged));
        private static readonly List<Listener> Listeners = new List<Listener>();
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
                Listener listener = Listeners.SingleOrDefault(l => l.TargetReference.Target == e.OldValue);
                if (listener != null)
                {
                    CollectionChangedEventManager.RemoveListener(source, listener);
                }
                ((IList)e.OldValue).Clear();
            }
            if (e.NewValue != null)
            {
                var listener = new Listener((IList)e.NewValue);
                Listeners.Add(listener);
                CollectionChangedEventManager.AddListener(source, listener);
                listener.ReceiveWeakEvent(typeof(CollectionChangedEventManager), source, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        internal class Listener : IWeakEventListener
        {
            internal readonly WeakReference TargetReference;
            public Listener(IList newValue)
            {
                TargetReference = new WeakReference(newValue);
            }

            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                var args = (NotifyCollectionChangedEventArgs)e;
                var target = (IList)TargetReference.Target;
                var source = (IList)sender;
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
                return true;
            }
        }
    }
}