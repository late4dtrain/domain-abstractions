namespace Late4dTrain.Domain.Abstractions.Tests.Specifications;

using System;

using AutoFixture.Xunit2;

using FluentAssertions;

using Late4dTrain.Domain.Abstractions;

using Xunit;

public class SpecificationTests
{
    [Theory]
    [AutoData]
    public void Composed_Eligibility_Rule_Uses_Boolean_Semantics(string name, int age, int creditScore)
    {
        var rule = new ActiveCustomerSpecification()
            .And(new AdultCustomerSpecification())
            .And(new CreditworthyCustomerSpecification());
        var candidate = new CustomerCandidate(name, IsActive: true, age, creditScore);

        rule.IsSatisfiedBy(candidate).Should().Be(IsExpectedEligible(age, creditScore));
    }

    [Fact]
    public void Or_And_Not_Follow_Logical_Short_Circuiting()
    {
        var calls = 0;
        var falseSpecification = new DelegateSpecification<int>(_ => false);
        var trueSpecification = new DelegateSpecification<int>(_ => true);
        var observedSpecification = new DelegateSpecification<int>(_ =>
        {
            calls++;
            return true;
        });

        falseSpecification.And(observedSpecification).IsSatisfiedBy(1).Should().BeFalse();
        calls.Should().Be(0);
        trueSpecification.Or(observedSpecification).IsSatisfiedBy(1).Should().BeTrue();
        calls.Should().Be(0);
        falseSpecification.Or(observedSpecification).IsSatisfiedBy(1).Should().BeTrue();
        calls.Should().Be(1);
        trueSpecification.Not().IsSatisfiedBy(1).Should().BeFalse();
    }

    [Fact]
    public void Null_Specification_Operands_Are_Rejected()
    {
        ISpecification<int> specification = new DelegateSpecification<int>(_ => true);
        ISpecification<int>? nullSpecification = null;

        FluentActions.Invoking(() => specification.And(nullSpecification!))
            .Should().Throw<ArgumentNullException>().WithParameterName("right");
        FluentActions.Invoking(() => nullSpecification!.Or(specification))
            .Should().Throw<ArgumentNullException>().WithParameterName("left");
        FluentActions.Invoking(() => nullSpecification!.Not())
            .Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void Null_Candidate_Is_Passed_Through_To_The_Domain_Predicate()
    {
        var specification = new NullCandidateSpecification();

        specification.Not().IsSatisfiedBy(null).Should().BeFalse();
    }

    private static bool IsExpectedEligible(int age, int creditScore) => age >= 18 && creditScore >= 650;

    private sealed record CustomerCandidate(string Name, bool IsActive, int Age, int CreditScore);

    private sealed class ActiveCustomerSpecification : ISpecification<CustomerCandidate>
    {
        public bool IsSatisfiedBy(CustomerCandidate candidate) => candidate.IsActive;
    }

    private sealed class AdultCustomerSpecification : ISpecification<CustomerCandidate>
    {
        public bool IsSatisfiedBy(CustomerCandidate candidate) => candidate.Age >= 18;
    }

    private sealed class CreditworthyCustomerSpecification : ISpecification<CustomerCandidate>
    {
        public bool IsSatisfiedBy(CustomerCandidate candidate) => candidate.CreditScore >= 650;
    }

    private sealed class DelegateSpecification<T> : ISpecification<T>
    {
        private readonly Func<T, bool> _predicate;

        public DelegateSpecification(Func<T, bool> predicate) => _predicate = predicate;

        public bool IsSatisfiedBy(T candidate) => _predicate(candidate);
    }

    private sealed class NullCandidateSpecification : ISpecification<string?>
    {
        public bool IsSatisfiedBy(string? candidate) => candidate is null;
    }
}
