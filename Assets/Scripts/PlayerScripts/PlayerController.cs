using System;
using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float speed;
        
        private Camera _cam;
        private Vector3 _mouseInput;
        private float _currentSpeed;

        private void Initialize()
        {
            _cam = Camera.main;
            _currentSpeed = speed;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Initialize();
        }

        private void Update()
        {
            if (!IsOwner || !Application.isFocused) return;
            
            _mouseInput.x = Input.mousePosition.x;
            _mouseInput.y = Input.mousePosition.y;
            _mouseInput.z = _cam.nearClipPlane;

            var mouseWorldCoords = _cam.ScreenToWorldPoint(_mouseInput);
            transform.position = Vector3.MoveTowards(transform.position, mouseWorldCoords, Time.deltaTime * _currentSpeed);
        }

        public void UpdateSpeed(float coefficient) => _currentSpeed = speed * coefficient;
    }
}
