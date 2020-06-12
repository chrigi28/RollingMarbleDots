using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Common.Scripts.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateAfter(typeof(StepPhysicsWorld))]
[UpdateBefore(typeof(EndFramePhysicsSystem))]
public class PlayerTriggerSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildworld;
    private StepPhysicsWorld stepworld;
    
    private struct PlayerCollisionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PlayerTagComponent> playerGroup;
        public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;
        
        public void Execute(CollisionEvent collisionEvent)
        {
            if (playerGroup.HasComponent(collisionEvent.Entities.EntityB) || playerGroup.HasComponent(collisionEvent.Entities.EntityA))
            {
                EventCenter.EnableJumpEvent.Invoke(new EnableJumpMessage());
            }
        }
    }

    protected override void OnCreate()
    {
        Enabled = true;
        buildworld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepworld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var task = new PlayerCollisionJob
        {
            playerGroup = GetComponentDataFromEntity<PlayerTagComponent>(true),
            PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
        };

        var handle = task.Schedule(stepworld.Simulation, ref buildworld.PhysicsWorld, inputDeps);

        return handle; 
    }
}
