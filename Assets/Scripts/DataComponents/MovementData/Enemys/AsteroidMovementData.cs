using Unity.Entities;

[GenerateAuthoringComponent]
public struct AsteroidMovementData : IComponentData
{
    public float Speed;

    public float TurnSpeed;
}