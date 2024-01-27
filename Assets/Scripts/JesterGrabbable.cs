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
            _jesterObject = GetComponentInParent<JesterObject>();
        }

        public override bool CanInteract()
        {
            if (_jesterObject.IsShowcasing || _jesterObject.IsAttached)
                return false;
            
            return base.CanInteract();
        }

        protected override void OnInteract()
        {
            var successAdd = _platform.AddToPlatform(_jesterObject);
            if (successAdd)
                _jesterObject.SetIsShowcasing(true);
        }
    }
}