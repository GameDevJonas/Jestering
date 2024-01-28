using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using Jestering.Input;
using Jestering.Interaction;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Jestering.Rating
{
    public class CourtManager : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _director;

        [SerializeField] private TimelineAsset _ratingTimeline;

        [SerializeField] private StudioEventEmitter _enterEmitter, _exitEmitter;

        [SerializeField] private Transform _showcasePoint;

        [SerializeField] private KingRating _rating;

        [SerializeField] private CinemachineVirtualCamera _normalPlayerCam, _courtCam;

        [SerializeField] private Transform _playerRespawnPoint;

        [SerializeField] private Transform _playerTransform, _platformTransform;

        [SerializeField] private JesterPlatform _platform;

        private JesterObject _currentlyRatingJesterObject;

        private int _complexity = 1;

        private bool ShouldShowRating = false;

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

            _exitEmitter.Play();

            _playerTransform.position = _playerRespawnPoint.position;
            _platformTransform.position = _playerTransform.position + _platformTransform.right;

            InputManager.EnablePlayerMap();

            _courtCam.enabled = false;
            _normalPlayerCam.enabled = true;
        }

        public void StartPresent()
        {
            var jesterObject = _platform.CurrentJester;
            if (!jesterObject)
                return;

            _enterEmitter.Play();

            _director.playableAsset = _ratingTimeline;
            _director.Play();

            StartCoroutine(StartPresentCoroutine(jesterObject));
        }

        private IEnumerator StartPresentCoroutine(JesterObject jesterObject)
        {
            InputManager.DisablePlayerMap();

            _normalPlayerCam.enabled = false;
            _courtCam.enabled = true;

            _currentlyRatingJesterObject = jesterObject;

            _currentlyRatingJesterObject.transform.SetParent(_showcasePoint);
            _currentlyRatingJesterObject.transform.position = _showcasePoint.position;

            while (!ShouldShowRating)
            {
                yield return null;
            }

            ShouldShowRating = false;

            _rating.ResetRequest();
            _platform.ResetPlatform();

            StartCoroutine(GetNewRequestCoroutine());
        }

        public void KingRateSignal()
        {
            _director.Pause();
            StartCoroutine(RateObject());
        }

        private Queue<int> matches = new Queue<int>();

        private IEnumerator RateObject()
        {
            _rating.OnCategoryMatch -= OnMatchCategory;
            _rating.OnCategoryMatch += OnMatchCategory;
            var success = _rating.RateObject(_currentlyRatingJesterObject, out int points);
            if (success)
                _complexity++;
            
            _rating.OnCategoryMatch -= OnMatchCategory;

            var hasFirstLaugh = false;
            var matchCount = matches.Count;
            for (int i = 0; i < matchCount; i++)
            {
                var match = matches.Dequeue();
                if (match > 0)
                {
                    _rating.SetKingFace(_rating.Faces.neutralFace);
                    yield return new WaitForSeconds(.1f);

                    var eventPath = hasFirstLaugh
                        ? "event:/Castle/funny_king_laugh"
                        : "event:/Castle/funny_king_laugh_first";
                    
                    RuntimeManager.PlayOneShot(eventPath);
                    
                    if (!hasFirstLaugh)
                    {
                        hasFirstLaugh = true;
                    }

                    _rating.SetKingFace(_rating.Faces.laughFace);


                }
                else if (match < 0)
                {
                    _rating.SetKingFace(_rating.Faces.hateFace);
                }

                _rating.UpdatePointsText(match);
                yield return new WaitForSeconds(1f);
            }
            
            _rating.SetKingFace(_rating.Faces.neutralFace);

            matches.Clear();
            
            if (success)
            {
                RuntimeManager.PlayOneShot("event:/Music/funny_fmusic_stinger_win");
            }
            else
            {
                RuntimeManager.PlayOneShot("event:/Music/funny_fmusic_stinger_lose");
            }

            yield return new WaitForSeconds(1);
            _director.Resume();
        }

        private void OnMatchCategory(int points)
        {
            matches.Enqueue(points);
        }



        public void ShowResultsSignal()
        {
            ShouldShowRating = true;
        }

    }
}