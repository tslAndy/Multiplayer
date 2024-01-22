using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace ServerScripts
{
    public class FoodSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject prefab;

        private const int MaxPrefabCount = 50;
        
        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
        }

        private void SpawnFoodStart()
        {
            NetworkManager.Singleton.OnServerStarted -= SpawnFoodStart;
            for (var i = 0; i < 30; i++)
            {
                SpawnFood();
            }

            StartCoroutine(SpawnOverTime());
        }

        private void SpawnFood()
        {
            var obj = NetworkObjectPool.Singleton.GetNetworkObject(prefab, GetRandomPosition(), Quaternion.identity);
            obj.GetComponent<Food>().prefab = prefab;
            if (!obj.IsSpawned) obj.Spawn(true);
        }

        private Vector3 GetRandomPosition()
        {
            var x = Random.Range(5, 17);
            var y = Random.Range(0, 10);
            return new Vector3(x, y, 0);
        }

        private IEnumerator SpawnOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                if (NetworkObjectPool.Singleton.GetCurrentPrefabCount(prefab) < MaxPrefabCount)
                {
                    SpawnFood();
                }
            }
        }
    }
}
