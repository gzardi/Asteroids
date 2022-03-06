using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    
    public float Speed;
    
    public float TurnSpeed;
    
    public float MaxImpulse;
}
