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
        [SerializeField] private int level = 1;

        private Dictionary<string, Entity> prefabEntities;
        private Dictionary<string, GameObject> prefabGameObjects;
        private BlobAssetStore blobAssetStore;
        private EntityManager EntityManager;
        private Random random;
        private float groundlenght;
        private float levelLength;

        void Awake()
        {
            random = new Random(1);
            this.EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            blobAssetStore = new BlobAssetStore();
            this.GetPrefabs();
            this.GenerateLevel(level);
        }

        void OnDestroy()
        {
            blobAssetStore.Dispose();
        }

        /// <summary>
        /// Get prefabs from Resourcefolder and store them in a dictionary as GO
        /// Also convert them to Entity and store in dictionary for entity prefab
        /// </summary>
        private void GetPrefabs()
        {
            prefabEntities = new Dictionary<string, Entity>();
            prefabGameObjects = new Dictionary<string, GameObject>();
            var prefabsToInitialize = new List<string> { "Floor", "Ball", "Cube" };

            foreach (var prefab in prefabsToInitialize)
            {
                GameObject ob = Resources.Load<GameObject>($"{prefab}");
                prefabGameObjects.Add(prefab, ob);
                World world = World.DefaultGameObjectInjectionWorld;
                var settings = GameObjectConversionSettings.FromWorld(world, blobAssetStore);
                prefabEntities.Add(prefab, GameObjectConversionUtility.ConvertGameObjectHierarchy(ob, settings));
            }
        }

        /// <summary>
        /// Instantiate a bunch of floors along the z axis
        /// </summary>
        /// <param name="level">handles the obstacle at later state</param>
        public void GenerateLevel(int level)
        {
            int numberOfFloors = 75;
            random = new Random((uint)level);
            this.GenerateFloor(numberOfFloors);
            this.GenerateObstacles(level);
            this.GeneratePlayer();
            this.GenerateFinish();
        }

        private void GenerateFinish()
        {
            
        }

        private void GenerateObstacles(int level)
        {
            var numberOfObstacles = 125;
            var obstacles = new NativeArray<Entity>(numberOfObstacles, Allocator.Temp);
            var prefabEntity = prefabEntities["Cube"];
            this.EntityManager.Instantiate(prefabEntity, obstacles);

            var firstLastPos = 12.5f;
            float3 min = new float3(-5, 2, firstLastPos);
            float3 max = new float3(5, 3, this.levelLength - firstLastPos);

            float3 cubesize;
            for (int z = 0; z < numberOfObstacles; z++)
            {
                this.EntityManager.AddComponent(obstacles[z], typeof(NonUniformScale));
                cubesize = random.NextFloat3(new float3(0.5f, 0.75f, 0.5f), new float3(6, 5, 3));
                this.EntityManager.AddComponentData(obstacles[z], new NonUniformScale { Value = cubesize });
                var newCollider = Unity.Physics.BoxCollider.Create(new BoxGeometry {Center = float3.zero, Size = cubesize, BevelRadius = 0f, Orientation = Quaternion.identity });
                this.EntityManager.SetComponentData(obstacles[z], new PhysicsCollider{Value = newCollider});
                this.EntityManager.SetComponentData(obstacles[z], new Translation { Value = random.NextFloat3(min, max) });
                this.EntityManager.SetComponentData(obstacles[z], new Rotation { Value = random.NextQuaternionRotation() });
            }

            obstacles.Dispose();
        }

        private void GenerateFloor(int numberOfFloors)
        {
            var grounds = new NativeArray<Entity>(numberOfFloors, Allocator.Temp);
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

            grounds.Dispose();
        }

        private void GeneratePlayer()
        {
            var entity = prefabEntities["Ball"];
            var ball = this.EntityManager.Instantiate(entity);
            this.EntityManager.SetComponentData(ball, new Translation { Value = new float3(0, 1, 0) });
        }
    }
}
