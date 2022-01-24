using AutoFixture.Xunit2;

using FluentAssertions;

using Xunit;

namespace Late4dTrain.Domain.Abstractions.Tests.ValueObject
{
    public class EqualityTests
    {
        [Theory]
        [AutoData]
        public void ValueObject_With_Same_Values_Will_Be_Equal(int first, int second, int third)
        {
            var valueObject1 = new TestValueObject(first, second, third);
            var valueObject2 = new TestValueObject(first, second, third);

            ReferenceEquals(valueObject1, valueObject2).Should().BeFalse();
            valueObject1.Equals(valueObject2).Should().BeTrue();
            (valueObject1 == valueObject2).Should().BeTrue();
            (valueObject1 != valueObject2).Should().BeFalse();
        }

        [Theory]
        [AutoData]
        public void ValueObject_With_Different_Values_Will_Be_UnEqual(
            int first1,
            int first2,
            int second,
            int third)
        {
            var valueObject1 = new TestValueObject(first1, second, third);
            var valueObject2 = new TestValueObject(first2, second, third);

            ReferenceEquals(valueObject1, valueObject2).Should().BeFalse();
            valueObject1.Equals(valueObject2).Should().BeFalse();
            (valueObject1 == valueObject2).Should().BeFalse();
            (valueObject1 != valueObject2).Should().BeTrue();
        }

        [Fact]
        public void ValueObject_Null_Equality()
        {
            TestValueObject? valueObject1 = null;
            TestValueObject? valueObject2 = null;
            (valueObject1 == valueObject2).Should().BeTrue();
            (valueObject1 != valueObject2).Should().BeFalse();
        }
    }
}
