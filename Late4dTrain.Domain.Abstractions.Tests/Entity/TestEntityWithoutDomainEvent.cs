namespace Late4dTrain.Domain.Abstractions.Tests.Entity.Data
{
    using System;

    internal class TestEntity
        : Entity<Guid>
    {
        public TestEntity(Guid id, string name) => (Id, Name) = (id, name);

        public string Name { get; }

        public sealed override Guid Id { get; protected set; }
    }
}
