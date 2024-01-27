using System;
using System.Collections;
using System.Collections.Generic;
using Jestering.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jestering.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _controller;

        [Header("Variables")]
        [SerializeField]
        private float _movementSpeed;

        private Vector2 _movementInputAxis;
        
        private void Awake()
        {
            _controller = GetComponentInParent<CharacterController>();
        }

        private void Start()
        {
            InputManager.RegisterInput(InputManager.Movement, OnMovementInput);
        }

        private void OnMovementInput(InputAction.CallbackContext callbackContext)
        {
            _movementInputAxis = callbackContext.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            var movement = _movementInputAxis * (_movementSpeed * Time.deltaTime);
            var fixedMovementAxis = new Vector3(movement.x, 0, movement.y);
            _controller.Move(fixedMovementAxis);
        }
    }
}