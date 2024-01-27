using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jestering.Interaction
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _rotationPivot;

        [SerializeField]
        private float _rotationSpeed;
        
        [SerializeField]
        private bool _onlyY;
        
        private Transform _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main.transform;
            if (!_rotationPivot)
                _rotationPivot = transform;
        }

        private void Update()
        {
            var targetRotation = Quaternion.LookRotation(_rotationPivot.position - _mainCamera.position);
            var targetRotEuler = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed > 0 ? _rotationSpeed * Time.deltaTime : 1)
                .eulerAngles;
            if (_onlyY)
            {
                targetRotEuler.x = transform.rotation.eulerAngles.x;
                targetRotEuler.z = transform.rotation.eulerAngles.z;
            }
            
            _rotationPivot.rotation = Quaternion.Euler(targetRotEuler);
        }
    }
}