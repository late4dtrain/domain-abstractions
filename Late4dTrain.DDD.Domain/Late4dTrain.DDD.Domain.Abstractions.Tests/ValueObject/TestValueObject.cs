namespace Late4dTrain.DDD.Domain.Abstractions.Tests.ValueObject;

using ValueObject = Abstractions.ValueObject;

internal class TestValueObject : ValueObject.AsClass
{
    public TestValueObject(int first, int second, int third) => (First, Second, Third) = (first, second, third);

    public int First { get; }

    public int Second { get; }

    public int Third { get; }
}
