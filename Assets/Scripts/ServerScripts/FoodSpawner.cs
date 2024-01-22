using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace ServerScripts
{
    public class FoodSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject prefab;

        private const int MaxPrefabCount = 80;
        
        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
        }

        private void SpawnFoodStart()
        {
            NetworkManager.Singleton.OnServerStarted -= SpawnFoodStart;
            for (var i = 0; i < MaxPrefabCount; i++)
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
            var x = Random.Range(0f, 35f);
            var y = Random.Range(0f, 19f);
            return new Vector3(x, y, 0);
        }

        private IEnumerator SpawnOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (NetworkObjectPool.Singleton.GetCurrentPrefabCount(prefab) < MaxPrefabCount)
                {
                    SpawnFood();
                }
            }
        }
    }
}
