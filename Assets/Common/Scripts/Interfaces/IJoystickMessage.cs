using UnityEngine;

namespace Assets.Common.Scripts.Components
{
    public interface IJoystickMessage : IMessageBase
    {
        Vector2 Movement { get; set; }
    }
}