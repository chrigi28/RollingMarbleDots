using Assets.Common.Scripts.Components;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Common.Scripts
{
    public interface ISetPlayerPositionMessage : IMessageBase
    {
        float3 Position { get; set; }
        Quaternion Rotation { get; set; }
        PhysicsVelocity Velocity { get; set; }
    }

    public class SetPlayerPositionMessage : ISetPlayerPositionMessage
    {
        public float3 Position { get; set; } = float3.zero;
        public Quaternion Rotation { get; set; } = Quaternion.identity;
        public PhysicsVelocity Velocity { get; set; } = new PhysicsVelocity {Angular = float3.zero, Linear = float3.zero};
    }

    [System.Serializable]
    public class SetPlayerPositionEvent : UnityEvent<SetPlayerPositionMessage>
    {
    }
}