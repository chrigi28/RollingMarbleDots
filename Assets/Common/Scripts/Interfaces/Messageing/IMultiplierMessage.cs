using Unity.Mathematics;
using UnityEngine;

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
}