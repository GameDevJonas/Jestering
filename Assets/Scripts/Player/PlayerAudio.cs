using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Jestering
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField]
        private StudioEventEmitter _footStepEmitter;
        
        public void DoFootStepAudio()
        {
            _footStepEmitter.Play();
        }
    }
}