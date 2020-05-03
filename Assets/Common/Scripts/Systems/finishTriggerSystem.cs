using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public class FinishTriggerSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildworld;
    private StepPhysicsWorld steworld;

    private struct TriggerJob : ITriggerEventsJob
    {
        public void Execute(TriggerEvent eventTrigger)
        {
            EventCenter.GameStateChangeEvent.Invoke(new GameStateChangeMessage{ Finish = true });
        }
    }

    protected override void OnCreate()
    {
        buildworld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        steworld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var a = new TriggerJob().Schedule(steworld.Simulation, ref buildworld.PhysicsWorld, inputDeps);
        a.Complete();
        return a; 
    }
}
