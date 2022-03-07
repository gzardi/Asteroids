using Unity.Entities;

[GenerateAuthoringComponent]
public struct HealthData : IComponentData
{
    public bool IsDead;
}