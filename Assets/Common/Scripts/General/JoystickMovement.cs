using Assets.Common.Scripts.Components;
using UnityEngine;

namespace Assets.Common.Scripts
{
    public class JoystickMovement : IJoystickMessage
    {
        public Vector2 Movement { get; set; }
    }
}