using PlayerScripts;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace ServerScripts
{
    public class Food : NetworkBehaviour
    {
        [SerializeField] private float mass;
        [HideInInspector] public GameObject prefab;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            if (!other.CompareTag("Player")) return;

            if (other.TryGetComponent(out PlayerGrowth playerGrowth))
            {
                playerGrowth.AddMassClientRpc(mass);
            }
            
            NetworkObject.Despawn();
        }
    }
}
