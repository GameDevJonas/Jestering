using System;
using System.Collections;
using System.Collections.Generic;
using Jestering.Movement;
using UnityEngine;

namespace Jestering
{
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerAudio _audio;
        private PlayerMovement _movement;
        private Animator _animator;
        
        
        private static readonly int Walking = Animator.StringToHash("Walking");

        private void Awake()
        {
            _audio = GetComponentInParent<PlayerAudio>();
            _movement = _audio.GetComponentInChildren<PlayerMovement>();
            _animator = GetComponent<Animator>();
        }

        public void FootStepSignal()
        {
            _audio.DoFootStepAudio();
        }

        private void Update()
        {
            SetIsWalking();
        }

        public void SetIsWalking()
        {
            _animator.SetBool(Walking, _movement.MovementInputAxis != Vector2.zero);
        }
    }
}