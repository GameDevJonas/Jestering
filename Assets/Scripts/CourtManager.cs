using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Jestering.Input;
using Jestering.Interaction;
using UnityEngine;

namespace Jestering.Rating
{
    public class CourtManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _showcasePoint;

        [SerializeField]
        private KingRating _rating;

        [SerializeField]
        private CinemachineVirtualCamera _normalPlayerCam, _courtCam;

        [SerializeField]
        private Transform _playerRespawnPoint;

        [SerializeField]
        private Transform _playerTransform, _platformTransform;

        [SerializeField]
        private JesterPlatform _platform;

        private int _complexity = 1;
        
        private void Start()
        {
            InputManager.DisablePlayerMap();
            StartCoroutine(GetNewRequestCoroutine());
        }

        private IEnumerator GetNewRequestCoroutine()
        {
            _normalPlayerCam.enabled = false;
            _courtCam.enabled = true;
            
            yield return new WaitForSeconds(1);

            _rating.NewRequest(_complexity);

            yield return new WaitForSeconds(1);

            _playerTransform.position = _playerRespawnPoint.position;
            _platformTransform.position = _playerTransform.position + _platformTransform.right;

            InputManager.EnablePlayerMap();
            
            _courtCam.enabled = false;
            _normalPlayerCam.enabled = true;
        }

        public void StartPresent()
        {
            var jesterObject = _platform.CurrentJester;
            if(!jesterObject)
                return;
            
            StartCoroutine(StartPresentCoroutine(jesterObject));
        }

        private IEnumerator StartPresentCoroutine(JesterObject jesterObject)
        {
            InputManager.DisablePlayerMap();
            
            _normalPlayerCam.enabled = false;
            _courtCam.enabled = true;
            
            yield return new WaitForSeconds(1);
            
            jesterObject.transform.SetParent(_showcasePoint);
            jesterObject.transform.position = _showcasePoint.position;
            
            var success = _rating.RateObject(jesterObject);
            if (success)
                _complexity++;
            
            yield return new WaitForSeconds(1);

            _rating.ResetRequest();
            _platform.ResetPlatform();
            
            StartCoroutine(GetNewRequestCoroutine());
        }
    }
}