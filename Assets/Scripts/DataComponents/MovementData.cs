using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    
    public float Speed;
    
    public float TurnSpeed;

    public KeyCode ForwardKey;
    
    public KeyCode BackwardKey;
    
    public KeyCode RightKey;
    
    public KeyCode LeftKey;
    
    public float MaxForce;
}
