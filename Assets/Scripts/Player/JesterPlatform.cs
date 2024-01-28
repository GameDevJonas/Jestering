using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Jestering.Interaction
{
    public class JesterPlatform : MonoBehaviour
    {
        [SerializeField]
        private StudioEventEmitter _platformAudio;
        
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

        private Rigidbody _rb;
        
        private JesterObject _currentJester;

        public JesterObject CurrentJester => _currentJester;

        private void Awake()
        {
            _playerObject = GameObject.FindWithTag("Player").transform;
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            PlatformAudio();
            RotateToPlayer();
            SetRopePositions();
        }

        private void PlatformAudio()
        {
            var isMoving = _rb.velocity.magnitude >= 1f;
            if (isMoving)
            {
                if(!_platformAudio.IsPlaying())
                    _platformAudio.Play();
            }
            else
            {
                if(_platformAudio.IsPlaying())
                    _platformAudio.Stop();
            }
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

        public void ResetPlatform()
        {
            Destroy(_currentJester.gameObject);
            _currentJester = null;
        }
    }
}