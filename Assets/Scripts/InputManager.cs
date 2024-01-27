using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jestering.Input
{
    [DefaultExecutionOrder(-10)]
    public class InputManager : MonoBehaviour
    {
        private static PlayerInput _input;

        public static InputAction Movement => _input.PlayerMap.Movement;
        public static InputAction Interact => _input.PlayerMap.Interact;

        private void Awake()
        {
            _input = new PlayerInput();
        }

        private void Start()
        {
            EnablePlayerMap();
        }

        public static void EnablePlayerMap()
        {
            _input.Enable();
        }

        public static void DisablePlayerMap()
        {
            _input.Disable();
        }
        
        public static void RegisterInput(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started += action;
            inputAction.performed += action;
            inputAction.canceled += action;
        }
        
        public static void UnregisterInput(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started -= action;
            inputAction.performed -= action;
            inputAction.canceled -= action;
        }
        
    }
}