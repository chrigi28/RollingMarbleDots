using System;
using System.Collections.Generic;
using Assets.Common.Scripts.Components;

class MessageCenter<T> where T : IMessageBase
{
    private static HashSet<IMessageReceiver<T>> RegisteredObjects = new HashSet<IMessageReceiver<T>>();

    public static void Register(IMessageReceiver<T> receiver)
    {
        RegisteredObjects.Add(receiver);
    }

    public static void UnRegister(IMessageReceiver<T> inst)
    {
        RegisteredObjects.Remove(inst);
    }

    public static void Send(T m)
    {
        foreach (var receiver in RegisteredObjects)
        {
            receiver.ExecuteMessage(m);
        }
    }
}

