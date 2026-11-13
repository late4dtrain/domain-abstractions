namespace Late4dTrain.Domain.Abstractions.Tests.Entity.Data
{
    using System;

    internal class TestEntity
        : Entity<Guid>
    {
        public TestEntity(Guid id, string name) : base(id) => Name = name;

        public string Name { get; }
    }
}
