using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Common.Scripts.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class MovementSpeed : MonoBehaviour
{
    [SerializeField] private float3 movementMultiplier = float3.zero;
    static MovementSpeed Instance;
    private Entity entity;
    private EntityManager manager = null;
    private float3 oldValue;
    private EntityQuery singletonGroup;
    
    void Awake()
    {
        if (Instance != null)
        {
            Instance.movementMultiplier = this.movementMultiplier;
            Destroy(this);
            return;
        }

        Instance = this;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entity = manager.CreateEntity(typeof(PhysicsSpeedMultiplier));
        manager.AddComponent(entity, typeof(PhysicsSpeedMultiplier));
    }

    void Update()
    {
        if (entity != Entity.Null && !oldValue.Equals(this.movementMultiplier)) 
        {
            manager.SetComponentData(entity, new PhysicsSpeedMultiplier {Value = movementMultiplier});
            oldValue = movementMultiplier;
            MessageCenter<IMultiplierMessage>.Send(new MultiplierMessage(){Multiplier = movementMultiplier});
        }
    }
}
