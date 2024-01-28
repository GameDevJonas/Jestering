using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Jestering
{
    [DefaultExecutionOrder(-50)]
    public class AudioSingleton : MonoBehaviour
    {
        public static AudioSingleton _instance;

        [SerializeField]
        private StudioEventEmitter _musEmitter, _ambxEmitter;
        
        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
            _musEmitter.Play();
            _ambxEmitter.Play();
        }
    }
}