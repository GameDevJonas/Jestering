using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jestering.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField]
        private float _frameBudget;
        
        [SerializeField]
        private GameObject _interactionSymbol;
        
        private List<Interactable> _interactables = new();

        private Interactable _bestInteractable;

        private void Update()
        {
            var frameCount = Time.frameCount;
            if(frameCount % _frameBudget != 0)
                return;
            
            UpdateInteractableList();
        }

        public void AddInteractable(Interactable interactable)
        {
            if (_interactables.Contains(interactable))
            {
                UpdateInteractableList();
                return;
            }
            
            _interactables.Add(interactable);
            UpdateInteractableList();
        }
        
        public void RemoveInteractable(Interactable interactable)
        {
            if (!_interactables.Contains(interactable))
            {
                UpdateInteractableList();
                return;
            }
            
            _interactables.Remove(interactable);
            UpdateInteractableList();
        }
        
        private void UpdateInteractableList()
        {
            if (_interactables.Count <= 0)
            {
                SetBestInteractable(null);
                return;
            }
            
            _interactables = _interactables.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
            SetBestInteractable(_interactables[0]);    
        }

        private void SetBestInteractable(Interactable interactable)
        {
            _bestInteractable = interactable;
            if (!_bestInteractable)
            {
                _interactionSymbol.transform.SetParent(null);
                _interactionSymbol.SetActive(false);
                return;
            }
            
            _interactionSymbol.SetActive(true);
            _interactionSymbol.transform.SetParent(interactable.InteractionPoint, false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponentInChildren<Interactable>();
            if (!interactable)
            {
                interactable = other.GetComponentInParent<Interactable>();
                if(!interactable)
                    return;
            }
            
            AddInteractable(interactable);
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.GetComponentInChildren<Interactable>();
            if (!interactable)
            {
                interactable = other.GetComponentInParent<Interactable>();
                if(!interactable)
                    return;
            }
            
            RemoveInteractable(interactable);
        }
    }

    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField]
        private Transform _interactionPoint;
        public Transform InteractionPoint => _interactionPoint;

        public abstract void Interact();
    }

    public class Grabbable : Interactable
    {
        
        
        public override void Interact()
        {
            
        }
    }
}