using UnityEngine;

namespace Assets.Common.Scripts.Components
{
    public interface IJoystickMessage : IMessageBase
    {
        Vector2 Movement { get; set; }
    }

    public class JoystickMovement : IJoystickMessage
    {
        public Vector2 Movement { get; set; }
    }
}