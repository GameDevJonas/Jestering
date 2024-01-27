using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jestering.Rating
{
    public class RequestCollectionUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _requestUIObjectPrefab;

        [SerializeField]
        private Transform _requestUIObjectParent;

        [SerializeField]
        private TextMeshProUGUI _pointsText;

        public void RemoveRequestUI()
        {
            foreach (var requestObject in _requestUIObjectParent.GetComponentsInChildren<RequestUIObject>())
            {
                if (Application.isPlaying)
                {
                    Destroy(requestObject.gameObject);
                }
                else
                {
                    DestroyImmediate(requestObject.gameObject);
                }
            }
        }

        public void InstantiateRequest(JesterObject.ItemCategory category, int points)
        {
            var instantiatedRequestUI = Instantiate(_requestUIObjectPrefab, _requestUIObjectParent);
            var requestUIObject = instantiatedRequestUI.GetComponent<RequestUIObject>();
            requestUIObject.AssignCategoryAndOverlay(category, points);
        }

        public void SetPointsText(int points)
        {
            _pointsText.SetText($"Funny points: {points}");
        }
    }
}