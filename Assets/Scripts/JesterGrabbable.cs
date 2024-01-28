using System;
using FMODUnity;
using UnityEngine;

namespace Jestering.Interaction
{
    public class JesterGrabbable : Interactable
    {
        [SerializeField]
        private StudioEventEmitter _grabAudio;
        
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
            {
                _jesterObject.SetCollisionEnabled(false);
                _jesterObject.SetIsShowcasing(true);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Category", (int)_jesterObject.Category - 1);
                _grabAudio.Play();
            }
        }
    }
}