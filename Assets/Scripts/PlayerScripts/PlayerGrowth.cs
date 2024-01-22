using System;
using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerGrowth : NetworkBehaviour
    {
        [SerializeField] private float startMass;

        private readonly NetworkVariable<float> _radius = new();
        public float Radius => _radius.Value;

        public float CurrentMass { get; private set; }
        private Vector3 _startScale;
        private PlayerController _controller;
        private SpriteRenderer _renderer;

        private void Start()
        {
            CurrentMass = startMass;
            _startScale = transform.localScale;
            _controller = GetComponent<PlayerController>();
            _renderer = GetComponent<SpriteRenderer>();
            
            if (IsOwner) UpdateRadiusServerRpc(_renderer.bounds.size.x);
        }

        [ClientRpc]
        public void AddMassClientRpc(float mass)
        {
            CurrentMass += mass;
            transform.localScale = _startScale * (CurrentMass / startMass);
            _controller.UpdateSpeed(startMass / CurrentMass);
            if (IsOwner) UpdateRadiusServerRpc(_renderer.bounds.size.x);

        }

        [ServerRpc]
        private void UpdateRadiusServerRpc(float radius)
        {
            //Debug.Log(radius);
            _radius.Value = radius;
        }
    }
}
