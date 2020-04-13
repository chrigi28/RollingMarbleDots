using System.Collections.Generic;
using System.Linq;
using Assets.Common.Scripts;
using Assets.Common.Scripts.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowPlayerSystem : ComponentSystem
{
    private IEnumerable<IReceiveEntityPosition> receivers;

    protected override void OnCreate()
    {
        receivers = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IReceiveEntityPosition>();
    }

    protected override void OnUpdate()
	{
        Entities.WithAny<PlayerTagComponent>().ForEach((ref Translation pos) =>
		{
            foreach (var receiver in receivers)
            {
                receiver.SendPosition(pos);
            }		
		});
	}
}