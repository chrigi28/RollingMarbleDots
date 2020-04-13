﻿using Assets.Common.Scripts.Components;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateAfter(typeof(InputGatheringSystem))]
class UpdatePlayerPositionSystem : ComponentSystem
{
    private float3 multiplier;

    protected override void OnCreate()
    {
        EntityManager.CreateEntity(typeof(CharacterControllerInput));
    }

    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        var inputData = EntityManager.GetComponentData<CharacterControllerInput>(GetSingletonEntity<CharacterControllerInput>());
        multiplier = EntityManager.GetComponentData<PhysicsSpeedMultiplier>(GetSingletonEntity<PhysicsSpeedMultiplier>()).Value;
    
        var move = new float3(inputData.Movement.x, inputData.Jumped ? 1 : 0, inputData.Movement.y);
        move *= multiplier * dt;
        inputData.Movement = float2.zero;
        Entities.WithAll<PlayerTagComponent>().ForEach((ref PhysicsVelocity _physicsVelocity, ref PhysicsMass _physicsMass) =>
        {

            ComponentExtensions.ApplyLinearImpulse(ref _physicsVelocity, _physicsMass, move);
        });
    }
}