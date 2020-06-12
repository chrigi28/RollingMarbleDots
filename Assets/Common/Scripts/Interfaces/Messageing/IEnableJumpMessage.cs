using UnityEngine;
using UnityEngine.Events;

namespace Assets.Common.Scripts.Components
{
    public interface IEnableJumpMessage : IMessageBase
    {
    }

    public class EnableJumpMessage : IEnableJumpMessage
    {
    }


    [System.Serializable]
    public class EnableJumpEvent : UnityEvent<EnableJumpMessage>
    {
    }
}