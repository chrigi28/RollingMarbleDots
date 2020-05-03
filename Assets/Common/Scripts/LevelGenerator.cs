using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = Unity.Mathematics.Random;

namespace Assets.Common.Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        private int noOfOb = 200;
        private Dictionary<string, Entity> prefabEntities;
        private Dictionary<string, GameObject> prefabGameObjects;
        private BlobAssetStore blobAssetStore;
        private EntityManager EntityManager;
        private Random random;
        private float groundlenght;
        private float levelLength;
        
        private Entity ball;
        private Entity finish;
        private NativeArray<Entity> grounds;
        private NativeArray<Entity> obstacles;

        void Awake()
        {
            random = new Random(1);
            this.EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            blobAssetStore = new BlobAssetStore();
            this.GetPrefabs();
            this.GenerateEntities();
            this.SetUpLevel(1, 125, 750f);
        }

        void OnDestroy()
        {
            this.grounds.Dispose();
            this.obstacles.Dispose();
            this.blobAssetStore.Dispose();
        }

        /// <summary>Get prefabs from Resourcefolder and store them in a dictionary as GO
        /// Also convert them to Entity and store in dictionary for entity prefab
        /// </summary>
        private void GetPrefabs()
        {
            prefabEntities = new Dictionary<string, Entity>();
            prefabGameObjects = new Dictionary<string, GameObject>();
            var prefabsToInitialize = new List<string> { "Floor", "Ball", "Cube", "FinishPrefab" };

            foreach (var prefab in prefabsToInitialize)
            {
                GameObject ob = Resources.Load<GameObject>($"{prefab}");
                prefabGameObjects.Add(prefab, ob);
                World world = World.DefaultGameObjectInjectionWorld;
                var settings = GameObjectConversionSettings.FromWorld(world, blobAssetStore);
                prefabEntities.Add(prefab, GameObjectConversionUtility.ConvertGameObjectHierarchy(ob, settings));
            }
        }

        /// <summary>Instantiate a bunch of floors along the z axis</summary>
        public void GenerateEntities()
        {
            this.GenerateFloors();
            this.GenerateObstacles();
            this.GeneratePlayer();
            this.GenerateFinish();
        }

        private void GenerateFinish(float zPos = 750f)
        {
            if (this.finish == Entity.Null)
            {
                var entity = prefabEntities["FinishPrefab"];
                this.finish = this.EntityManager.Instantiate(entity);
            }

            this.EntityManager.SetComponentData(this.finish, new Translation { Value = new float3(0, 0.01f, zPos) });
            this.EntityManager.SetComponentData(this.finish, new Rotation { Value = Quaternion.Euler(0, 0, 0) });
        }

        private void GenerateObstacles()
        {
            this.obstacles = new NativeArray<Entity>(this.noOfOb, Allocator.Persistent);
            var prefabEntity = prefabEntities["Cube"];
            this.EntityManager.Instantiate(prefabEntity, this.obstacles);
            this.DisableEntities(this.obstacles);
        }

        private void DisableEntities(NativeArray<Entity> entities)
        {
            foreach (var entity in entities)
            {
                this.EntityManager.SetEnabled(entity, false);
            }
        }

        private void GenerateFloors(int numberOfFloors = 100)
        {
            this.grounds = new NativeArray<Entity>(numberOfFloors, Allocator.Persistent);
            var entity = prefabEntities["Floor"];
            this.EntityManager.Instantiate(entity, grounds);
            this.groundlenght = prefabGameObjects["Floor"].transform.localScale.z;
            this.levelLength = numberOfFloors * groundlenght;

            float3 pos;
            for (int z = 0; z < numberOfFloors; z++)
            {
                pos = float3.zero;
                pos.z = z * groundlenght;
                this.EntityManager.SetComponentData(grounds[z], new Translation { Value = pos });
                ////this.EntityManager.SetComponentData(grounds[y], new Rotation { Value = Quaternion.Euler(tempRot) });
            }
        }

        private void GeneratePlayer()
        {
            var entity = prefabEntities["Ball"];
            this.ball = this.EntityManager.Instantiate(entity);
            this.EntityManager.SetComponentData(ball, new Translation { Value = new float3(0, 1, 0) });
        }

        public void SetUpLevel(int level, int numberOfObstacles, float levelLength, float firstLastPos = 12.5f)
        {
            this.EnableGrounds(levelLength);

            random = new Random((uint)level);
            float3 min = new float3(-5, 2, firstLastPos);
            float3 max = new float3(5, 3, levelLength - firstLastPos);

            float3 cubesize;
            for (int z = 0; z < numberOfObstacles; z++)
            {
                var obstacle = this.obstacles[z];
                cubesize = random.NextFloat3(new float3(0.5f, 0.75f, 0.5f), new float3(6, 5, 3));
                this.EntityManager.AddComponentData(obstacle, new NonUniformScale { Value = cubesize });
                var newCollider = Unity.Physics.BoxCollider.Create(new BoxGeometry { Center = float3.zero, Size = cubesize, BevelRadius = 0f, Orientation = Quaternion.identity });
                this.EntityManager.SetComponentData(obstacle, new PhysicsCollider { Value = newCollider });
                this.EntityManager.SetComponentData(obstacle, new Translation { Value = random.NextFloat3(min, max) });
                this.EntityManager.SetComponentData(obstacle, new Rotation { Value = random.NextQuaternionRotation() });
                this.EntityManager.SetEnabled(obstacle, true);
            }
        }

        private void EnableGrounds(float levelLength)
        {
            var numberOfGrounds = levelLength / this.groundlenght;
            numberOfGrounds++;

            var index = 0;
            foreach (var ground in this.grounds)
            {
                this.EntityManager.SetEnabled(ground, index < numberOfGrounds);
            }
        }
    }
}
