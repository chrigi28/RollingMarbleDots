using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Common.Scripts.Components
{
    public interface IMultiplierMessage : IMessageBase
    {
        float3 Multiplier { get; set; }
    }

    public class MultiplierMessage : IMultiplierMessage
    {
        public float3 Multiplier { get; set; }
    }

    [System.Serializable]
    public class MultiplierChangedEvent : UnityEvent<MultiplierMessage>
    {
    }
}