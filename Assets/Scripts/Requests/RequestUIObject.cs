using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Jestering.Rating
{
    public class RequestUIObject : MonoBehaviour
    {
        [Serializable]
        public class CategoryImages
        {
            public Sprite Girlypop;
            public Sprite Obscure;
            public Sprite Koselig;
            public Sprite Cool;
            public Sprite Provocative;
            public Sprite ComedyGold;
        }

        [Serializable]
        public class OverlayImages
        {
            public Sprite LoveOverlay;
            public Sprite LikeOverlay;
            public Sprite DislikeOverlay;
            public Sprite HateOverlay;
        }

        [SerializeField]
        private CategoryImages _categoryImages;
        
        [SerializeField]
        private OverlayImages _overlayImages;

        [SerializeField]
        private Image _categoryImage;

        [SerializeField]
        private Image _overlayImage;

        public void AssignCategoryAndOverlay(JesterObject.ItemCategory category, int points)
        {
            switch (category)
            {
                case JesterObject.ItemCategory.Girlypop:
                    _categoryImage.sprite = _categoryImages.Girlypop;
                    break;
                case JesterObject.ItemCategory.Obscure:
                    _categoryImage.sprite = _categoryImages.Obscure;
                    break;
                case JesterObject.ItemCategory.Koselig:
                    _categoryImage.sprite = _categoryImages.Koselig;
                    break;
                case JesterObject.ItemCategory.Cool:
                    _categoryImage.sprite = _categoryImages.Cool;
                    break;
                case JesterObject.ItemCategory.Provocative:
                    _categoryImage.sprite = _categoryImages.Provocative;
                    break;
                case JesterObject.ItemCategory.ComedyGold:
                    _categoryImage.sprite = _categoryImages.ComedyGold;
                    break;
            }

            switch (points)
            {
                case 2:
                    _overlayImage.sprite = _overlayImages.LoveOverlay;
                    break;
                case 1:
                    _overlayImage.sprite = _overlayImages.LikeOverlay;
                    break;
                case -1:
                    _overlayImage.sprite = _overlayImages.DislikeOverlay;
                    break;
                case -2:
                    _overlayImage.sprite = _overlayImages.HateOverlay;
                    break;
            }
        }
    }
}