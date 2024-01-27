using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jestering.Rating
{
    public class CourtEntrance : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onTriggerEvent;

        private bool _hasTriggered;
        
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player") || _hasTriggered)
                return;
            
            _onTriggerEvent.Invoke();
            _hasTriggered = true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("Player") || !_hasTriggered)
                return;
            
            // _onTriggerEvent.Invoke();
            _hasTriggered = false;
        }
    }
}