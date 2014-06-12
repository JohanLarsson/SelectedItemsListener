namespace SelectedItemsListener
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public static class ComboBoxExt
    {
        public static readonly DependencyProperty HighlightedItemProperty = DependencyProperty.RegisterAttached(
            "HighlightedItem",
            typeof (object),
            typeof (ComboBoxExt),
            new PropertyMetadata(default(object), OnHighligtedChanged));

        public static void SetHighlightedItem(Selector element, object value)
        {
            element.SetValue(HighlightedItemProperty, value);
        }

        public static object GetHighlightedItem(Selector element)
        {
            return (object) element.GetValue(HighlightedItemProperty);
        }

        private static void OnHighligtedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
        }
    }
}