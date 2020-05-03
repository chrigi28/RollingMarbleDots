using Assets;
using Assets.Common.Scripts;
using Assets.Common.Scripts.Components;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateAfter(typeof(InputGatheringSystem))]
class UpdatePlayerPositionSystem : ComponentSystem
{
    private float3 multiplier = float3.zero;
    
    protected override void OnCreate()
    {
        EntityManager.CreateEntity(typeof(CharacterControllerInput));
        EventCenter.MultiplierChangedEvent.AddListener(m => this.ChangeMultiplier(m));
        EventCenter.PlayerPositionChangedEvent.AddListener(m => this.SetPlayerPosition(m));
    }

    protected override void OnUpdate()
    {
        if (GameManager.Instance.IsRunning())
        {
            var dt = Time.DeltaTime;
            var inputData = EntityManager.GetComponentData<CharacterControllerInput>(GetSingletonEntity<CharacterControllerInput>());

            var move = new float3(inputData.Movement.x, inputData.Jumped ? 1 : 0, inputData.Movement.y);
            move *= multiplier * dt;
            inputData.Movement = float2.zero;
            Entities.WithAll<PlayerTagComponent>().ForEach(
                (ref PhysicsVelocity _physicsVelocity, ref PhysicsMass _physicsMass) =>
                {
                    ComponentExtensions.ApplyLinearImpulse(ref _physicsVelocity, _physicsMass, move);
                });
        }
    }

    public void ChangeMultiplier(MultiplierMessage m)
    {
        multiplier = m.Multiplier;
    }

    public void SetPlayerPosition(SetPlayerPositionMessage m)
    {
        Entities.WithAny<PlayerTagComponent>().ForEach((ref Translation pos, ref Rotation rot, ref PhysicsVelocity velo) =>
        {
            pos.Value = m.Position;
            rot.Value = m.Rotation;
            velo.Angular = m.Velocity.Angular;
            velo.Linear= m.Velocity.Linear;
        });
    }
}