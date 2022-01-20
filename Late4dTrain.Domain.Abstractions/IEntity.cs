namespace Late4dTrain.DDD.Domain.Abstractions;

public interface IEntity<out TId>
{
    public TId Id { get; }
}
