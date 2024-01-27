using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            Request GetRequestFromIndex(int index)
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

        public void ResetRequest()
        {
            _requestCollectionUI.RemoveRequestUI();
            _currentRequest = null;
        }

        public void RateObject(JesterObject jesterObject)
        {
            
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