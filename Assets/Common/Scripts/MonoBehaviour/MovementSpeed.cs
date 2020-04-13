using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        var ent = manager.CreateEntity(typeof(PhysicsSpeedMultiplier));
        manager.AddComponent(ent, typeof(PhysicsSpeedMultiplier));
        singletonGroup = manager.CreateEntityQuery(typeof(PhysicsSpeedMultiplier));
        singletonGroup.SetSingleton<PhysicsSpeedMultiplier>(new PhysicsSpeedMultiplier {Value = this.movementMultiplier});
    }

    void Update()
    {
        if (!oldValue.Equals(this.movementMultiplier))
        {
            manager.SetComponentData(entity, new PhysicsSpeedMultiplier {Value = movementMultiplier});
            oldValue = movementMultiplier;
            singletonGroup.SetSingleton(new PhysicsSpeedMultiplier{Value = this.movementMultiplier});
        }
    }
}
