using UnityEngine;
using UnityEngine.Events;

namespace Assets.Common.Scripts.Components
{
    public interface IGameStateChangeMessage : IMessageBase
    {
        bool GameOver { get; set; }
        bool Pause { get; set; }
        bool Finish { get; set; }
    }

    public class GameStateChangeMessage : IGameStateChangeMessage
    {
        public bool GameOver { get; set; }
        public bool Pause { get; set; }
        public bool Finish { get; set; }
    }


    [System.Serializable]
    public class GameStateChangeEvent : UnityEvent<GameStateChangeMessage>
    {
    }
}