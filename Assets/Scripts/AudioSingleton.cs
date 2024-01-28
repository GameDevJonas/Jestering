using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jestering
{
    [DefaultExecutionOrder(-50)]
    public class AudioSingleton : MonoBehaviour
    {
        private AudioSingleton _instance;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
}