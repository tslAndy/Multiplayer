using System;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerCollisions : NetworkBehaviour
    {
        private PlayerGrowth _playerGrowth;

        [CanBeNull] private PlayerGrowth _otherGrowth;

        private void Start()
        {
            _playerGrowth = GetComponent<PlayerGrowth>();
        }

        private void Update()
        {
            if (_otherGrowth is null) return;
            
            var thisR = _playerGrowth.Radius;
            var otherR = _otherGrowth.Radius;
            if (thisR/ otherR < 2) return;

            var distance = Vector3.Distance(transform.position, _otherGrowth.transform.position);
            Debug.Log($"{distance} {otherR} {thisR}");
            if (!(distance + otherR < thisR)) return;
            _playerGrowth.AddMassClientRpc(_otherGrowth.CurrentMass);
            _otherGrowth.NetworkObject.Despawn();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!NetworkManager.IsServer) return;
            if (!other.CompareTag("Player")) return;
            _otherGrowth = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!NetworkManager.IsServer) return;
            if (!other.CompareTag("Player")) return;
            _otherGrowth = other.GetComponent<PlayerGrowth>();

        }
    }
}
