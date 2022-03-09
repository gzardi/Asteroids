using Unity.Entities;

[GenerateAuthoringComponent]
public struct HealthData : IComponentData
{
    public int Value;
    public bool IsDead;
}