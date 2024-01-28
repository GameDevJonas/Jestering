using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jestering.Rating
{
    public class KingRating : MonoBehaviour
    {
        [SerializeField]
        private RequestCollectionUI _requestCollectionUI;

        [SerializeField]
        private KingRequest _currentRequest;

        [SerializeField]
        private SpriteRenderer _kingfaceSpriteRenderer;

        [SerializeField]
        private KingFaces _faces;
        public KingFaces Faces => _faces;

        [Serializable]
        public class KingFaces
        {
            public Sprite neutralFace;
            public Sprite laughFace;
            public Sprite loveFace;
            public Sprite likeFace;
            public Sprite dislikeFace;
            public Sprite hateFace;
        }
        
        public KingRequest CurrentRequest => _currentRequest;

        private int _points;

        public void SetKingFace(Sprite face)
        {
            _kingfaceSpriteRenderer.sprite = face;

            // switch (face)
            // {
            //     case "neutral":
            //         _kingfaceSpriteRenderer.sprite = _faces.
            //         break;
            //     case "love":
            //         break;
            //     case "like":
            //         break;
            //     case "dislike":
            //         break;
            //     case "hate":
            //         break;
            // }
        }

        public void NewRequest(int complexity)
        {
            ResetRequest();
            _currentRequest = new KingRequest(complexity);
            for (int i = 0; i < 4; i++)
            {
                if(i >= complexity)
                    return;

                var request = GetRequestFromIndex(i);
                if(request.category == JesterObject.ItemCategory.None)
                    continue;
                
                var category = request.category;
                var point = request.points;

                _requestCollectionUI.InstantiateRequest(category, point);
            }
        }

        public void ResetRequest()
        {
            _requestCollectionUI.RemoveRequestUI();
            _currentRequest = null;
        }
        
        public bool RateObject(JesterObject jesterObject, out int points)
        {
            points = 0;
            
            if (!jesterObject || _currentRequest.points != 0)
            {
                _currentRequest.points = 0;
                return false;
            }

            List<JesterObject.ItemCategory> attachedCategories = new List<JesterObject.ItemCategory>();
            
            CheckAttachedJesterObject(jesterObject, attachedCategories);
            
            foreach (var attachedCategory in attachedCategories)
            {
                bool success = false;
                
                for (int i = 0; i < 5; i++)
                {
                    var request = GetRequestFromIndex(i);
                    if (attachedCategory == request.category)
                    {
                        _currentRequest.points += request.points;
                        success = true;
                    }
                }
                
                Debug.Log($"Category: {attachedCategory}. Points are now {_currentRequest.points}");
            }

            var currentRequestPoints = _currentRequest.points;

            _points += currentRequestPoints;
            points = currentRequestPoints;
            return currentRequestPoints >= 2;
        }

        public void UpdatePointsText()
        {
            _requestCollectionUI.SetPointsText(_points);
        }
        
        private static void CheckAttachedJesterObject(JesterObject jesterObject, List<JesterObject.ItemCategory> attachedCategories)
        {
            attachedCategories.Add(jesterObject.Category);

            var attachedHeadObject = jesterObject.Slots.HeadSlot.attachedJesterObject;
            if (attachedHeadObject)
                attachedCategories.Add(attachedHeadObject.Category);

            var attachedFaceObject = jesterObject.Slots.FaceSlot.attachedJesterObject;
            if (attachedFaceObject)
                attachedCategories.Add(attachedFaceObject.Category);

            var attachedLeftArmObject = jesterObject.Slots.LeftArmSlot.attachedJesterObject;
            if (attachedLeftArmObject)
                attachedCategories.Add(attachedLeftArmObject.Category);

            var attachedRightArmObject = jesterObject.Slots.RightArmSlot.attachedJesterObject;
            if (attachedRightArmObject)
                attachedCategories.Add(attachedRightArmObject.Category);
        }
        
        public Request GetRequestFromIndex(int index)
        {
            if (index <= 0)
                return _currentRequest.LoveRequest;
            else if (index <= 1)
                return _currentRequest.LikeRequest;
            else if (index <= 2)
                return _currentRequest.DislikeRequest;
            else if (index <= 3)
                return _currentRequest.HateRequest;

            return new Request(JesterObject.ItemCategory.None, 0);
        }
    }

    //LOVE request
    //Like request
    //Dislike request
    //HATE request

    [Serializable]
    public class KingRequest
    {
        public Request LoveRequest;
        public Request LikeRequest;
        public Request DislikeRequest;
        public Request HateRequest;

        public int points = 0;
        
        public KingRequest(int complexity)
        {
            var loveCategory = GetRandomCategory(null);
            LoveRequest = new Request(loveCategory, 2);
            if(complexity <= 1)
                return;
            
            var likeCategory = GetRandomCategory(new[] { loveCategory });
            LikeRequest = new Request(likeCategory, 1);
            if (complexity <= 2)
                return;
            
            var dislikeCategory = GetRandomCategory(new []{loveCategory, likeCategory});
            DislikeRequest = new Request(dislikeCategory,-1);
            if (complexity <= 3)
                return;
            
            var hateCategory = GetRandomCategory(new[] { loveCategory, dislikeCategory, likeCategory });
            HateRequest = new Request(hateCategory, -2);
        }

        private JesterObject.ItemCategory GetRandomCategory(JesterObject.ItemCategory[] avoidCategories)
        {
            var randomNum = Random.Range(1, 7);
            var randomCategory = (JesterObject.ItemCategory)randomNum;
            if (avoidCategories == null || !avoidCategories.Contains(randomCategory))
                return randomCategory;

            return GetRandomCategory(avoidCategories);
        }
    }
    
    [Serializable]
    public struct Request
    {
        public JesterObject.ItemCategory category;
        public int points;

        public Request(JesterObject.ItemCategory category, int points)
        {
            this.category = category;
            this.points = points;
        }
    }
}