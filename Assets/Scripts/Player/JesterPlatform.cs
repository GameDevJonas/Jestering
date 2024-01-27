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

        private JesterObject _currentJester;
        
        private void Update()
        {
            _rope.SetPosition(0, _startPoint.position);
            _rope.SetPosition(1, _endPoint.position);
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