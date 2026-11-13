namespace Late4dTrain.Domain.Abstractions.Tests.ValueObject.Data
{
    using System.Collections.Generic;

    using ValueObject = Abstractions.ValueObject;

    internal class TestValueObject : ValueObject
    {
        public TestValueObject(int first, int second, int third, string description = "") =>
            (First, Second, Third, Description) = (first, second, third, description);

        public int First { get; }

        public int Second { get; }

        public int Third { get; }

        public string Description { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return First;
            yield return Second;
            yield return Third;
        }
    }
}
