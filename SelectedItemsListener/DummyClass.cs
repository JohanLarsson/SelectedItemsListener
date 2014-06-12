namespace SelectedItemsListener
{
    public class DummyClass
    {
        public DummyClass(int value)
        {
            Value = value;
        }
        public int Value { get; private set; }

        public override string ToString()
        {
            return string.Format("Value: {0}", Value);
        }
    }
}