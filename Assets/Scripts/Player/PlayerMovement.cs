using System;
using System.Collections;
using System.Collections.Generic;
using Jestering.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jestering.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        private const float GRAVITY = -9.81f;
        
        private CharacterController _controller;

        [Header("Variables")]
        [SerializeField]
        private float _movementSpeed;

        private Vector2 _movementInputAxis;
        
        private void Awake()
        {
            _controller = GetComponentInParent<CharacterController>();
        }

        private void OnEnable()
        {
            InputManager.RegisterInput(InputManager.Movement, OnMovementInput);
        }

        private void OnDisable()
        {
            InputManager.UnregisterInput(InputManager.Movement, OnMovementInput);
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
            var fixedMovementAxis = new Vector3(movement.x, _controller.isGrounded ? 0 : GRAVITY, movement.y);
            _controller.Move(fixedMovementAxis);
        }
    }
}