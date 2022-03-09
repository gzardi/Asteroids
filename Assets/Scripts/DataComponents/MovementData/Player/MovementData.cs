using Unity.Entities;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    
    public float Speed;
    
    public float TurnSpeed;
    
    public float MaxImpulse;
}
