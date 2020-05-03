using System.Collections.Generic;
using System.Linq;
using Assets;
using Assets.Common.Scripts;
using Assets.Common.Scripts.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CheckPlayerPositionSystem : ComponentSystem
{
    protected override void OnUpdate()
	{
        if (GameManager.Instance.IsRunning())
        {
            Entities.WithAny<PlayerTagComponent>().ForEach((ref Translation pos) =>
            {
                if (pos.Value.y < -1.5f)
                {
                    EventCenter.GameStateChangeEvent.Invoke(new GameStateChangeMessage() {GameOver = true});
                }
            });
        }
    }
}