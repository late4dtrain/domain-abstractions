namespace Late4dTrain.Domain.Abstractions.Tests.ValueObject.Data
{
    using ValueObject = Abstractions.ValueObject;

    internal class TestValueObject : ValueObject
    {
        public TestValueObject(int first, int second, int third) => (First, Second, Third) = (first, second, third);

        public int First { get; }

        public int Second { get; }

        public int Third { get; }
    }
}
