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

    private static MovementSpeed Instance;
    private Entity entity;
    private EntityManager manager;
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
        entity = manager.CreateEntity(ComponentType.ReadOnly<PhysicsSpeedMultiplier>());
    }

    void Update()
    {
        manager.SetComponentData(entity, new PhysicsSpeedMultiplier { Value = movementMultiplier });
    }
}
