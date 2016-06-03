namespace Brisk.Tests
{
    public class TestItem : Entity
    {
        public string Name { get; set; }
    }

    public class TestSubclass : TestItem
    {
        public int SomeValue { get; set; }
    }
}