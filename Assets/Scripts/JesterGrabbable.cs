using System;
using UnityEngine;

namespace Jestering.Interaction
{
    public class JesterGrabbable : Interactable
    {
        private JesterPlatform _platform;

        private JesterObject _jesterObject;
        
        private void Awake()
        {
            _platform = FindObjectOfType<JesterPlatform>();
            _jesterObject = GetComponentInChildren<JesterObject>();
        }

        protected override void OnInteract()
        {
            var successAdd = _platform.AddToPlatform(_jesterObject);
            Debug.Log($"{_jesterObject.name} was added to platform with result {successAdd}", this);
        }
    }
}