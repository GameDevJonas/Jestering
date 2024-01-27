using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jestering.Interaction
{
    public class JesterPlatform : MonoBehaviour
    {
        [Header("Rope")]
        [SerializeField]
        private LineRenderer _rope;

        [SerializeField]
        private Transform _startPoint, _endPoint;

        [Header("Jester refs")]
        [SerializeField]
        private Transform _showcasePoint;

        [Header("Rotation")]
        [SerializeField]
        private float _rotationSpeed;

        private Transform _playerObject;
        
        private JesterObject _currentJester;

        private void Awake()
        {
            _playerObject = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            RotateToPlayer();
            SetRopePositions();
        }

        private void SetRopePositions()
        {
            _rope.SetPosition(0, _startPoint.position);
            _rope.SetPosition(1, _endPoint.position);
        }

        private void RotateToPlayer()
        {
            var targetRotation = Quaternion.LookRotation(transform.position - _playerObject.position);
            var targetRotEuler = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime).eulerAngles;
            targetRotEuler.x = transform.rotation.eulerAngles.x;
            targetRotEuler.z = transform.rotation.eulerAngles.z;
            
            transform.rotation = Quaternion.Euler(targetRotEuler);
        }

        public bool AddToPlatform(JesterObject jesterObject)
        {
            if (!_currentJester)
            {
                jesterObject.transform.SetParent(_showcasePoint);
                jesterObject.transform.position = _showcasePoint.position;
                _currentJester = jesterObject;
                return true;
            }
            
            var success = _currentJester.AttachToMe(jesterObject);
            return success;
        }
    }
}