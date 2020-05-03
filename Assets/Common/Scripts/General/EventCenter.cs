using System;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.Common.Scripts;
using Assets.Common.Scripts.Components;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;

class EventCenter
{
    public static SetPlayerPositionEvent PlayerPositionChangedEvent = new SetPlayerPositionEvent();
    public static GameStateChangeEvent GameStateChangeEvent = new GameStateChangeEvent();
    public static MultiplierChangedEvent MultiplierChangedEvent = new MultiplierChangedEvent();

}

