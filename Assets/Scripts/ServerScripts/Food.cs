using Unity.Netcode;
using UnityEngine;
using Utils;

namespace ServerScripts
{
    public class Food : NetworkBehaviour
    {
        [SerializeField] public GameObject prefab;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            if (!other.CompareTag("Player")) return;
            NetworkObject.Despawn();
        }
    }
}
