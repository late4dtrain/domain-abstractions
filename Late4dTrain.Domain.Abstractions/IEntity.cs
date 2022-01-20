namespace Late4dTrain.Domain.Abstractions;

public interface IEntity<out TId>
{
    public TId Id { get; }
}
