using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Rendering;

namespace Assets.Common.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct CharacterControllerInput : IComponentData
    {
        public float2 Movement;
        public bool Jumped;
    }

}