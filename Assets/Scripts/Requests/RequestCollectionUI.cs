using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jestering.Rating
{
    public class RequestCollectionUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _requestUIObjectPrefab;

        [SerializeField]
        private Transform _requestUIObjectParent;

        public void RemoveRequestUI()
        {
            foreach (var requestObject in _requestUIObjectParent.GetComponentsInChildren<RequestUIObject>())
            {
                if (Application.isPlaying)
                {
                    Destroy(requestObject);
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
    }
}