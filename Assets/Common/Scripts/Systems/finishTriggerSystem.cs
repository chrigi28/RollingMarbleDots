using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public class FinishTriggerSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildworld;
    private StepPhysicsWorld steworld;

    private struct FinishTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<FinishTagComponent> finishGroup;
        [ReadOnly] public ComponentDataFromEntity<PlayerTagComponent> playerGroup;

        public void Execute(TriggerEvent eventTrigger)
        {
            if (finishGroup.HasComponent(eventTrigger.Entities.EntityA) && playerGroup.HasComponent(eventTrigger.Entities.EntityB) || finishGroup.HasComponent(eventTrigger.Entities.EntityB) && playerGroup.HasComponent(eventTrigger.Entities.EntityA) )
            {
                EventCenter.GameStateChangeEvent.Invoke(new GameStateChangeMessage {Finish = true});
            }
        }
    }

    protected override void OnCreate()
    {
        buildworld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        steworld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var a = new FinishTriggerJob
        {
            finishGroup = GetComponentDataFromEntity<FinishTagComponent>(true),
            playerGroup = GetComponentDataFromEntity<PlayerTagComponent>(true)
        };

        a.Schedule(steworld.Simulation, ref buildworld.PhysicsWorld, inputDeps);
        return inputDeps; 
    }
}
