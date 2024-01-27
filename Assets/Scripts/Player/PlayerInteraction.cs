using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jestering.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jestering.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField]
        private float _frameBudget;

        [SerializeField]
        private bool _interactionSymbolOnInteractable = false;
        
        [SerializeField]
        private GameObject _interactionSymbol;
        
        private List<Interactable> _interactables = new();

        private Interactable _bestInteractable;

        private void Start()
        {
            SetBestInteractable(null);
        }

        private void OnEnable()
        {
            InputManager.RegisterInput(InputManager.Interact, OnInteractInput);
        }

        private void OnDisable()
        {
            InputManager.UnregisterInput(InputManager.Interact, OnInteractInput);
        }

        private void Update()
        {
            var frameCount = Time.frameCount;
            if(frameCount % _frameBudget != 0)
                return;
            
            UpdateInteractableList();
        }

        private void OnInteractInput(InputAction.CallbackContext context)
        {
            if(!context.performed)
                return;

            if(!_bestInteractable)
                return;

            if(!_bestInteractable.CanInteract())
                return;
            
            _bestInteractable.Interact();
            UpdateInteractableList();
        }
        
        #region Interctables management
        
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

            for (var index = 0; index < _interactables.Count; index++)
            {
                var interactable = _interactables[index];
                if (interactable == null)
                    _interactables.Remove(interactable);
            }

            _interactables = _interactables.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
            if (_interactables.All(x => !x.CanInteract()))
            {
                SetBestInteractable(null);
            }

            var firstInteractable = _interactables.FirstOrDefault(x => x.CanInteract());
            SetBestInteractable(firstInteractable ? firstInteractable : null);    
        }

        private void SetBestInteractable(Interactable interactable)
        {
            _bestInteractable = interactable;
            if (!_bestInteractable)
            {
                if(_interactionSymbolOnInteractable)
                    _interactionSymbol.transform.SetParent(null);
                
                _interactionSymbol.SetActive(false);
                return;
            }
            
            _interactionSymbol.SetActive(true);
            
            if(!_interactionSymbolOnInteractable)
                return;
            
            _interactionSymbol.transform.SetParent(interactable.InteractionPoint, false);
            _interactionSymbol.transform.position = interactable.InteractionPoint.position;
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
        #endregion

    }

    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField]
        private Transform _interactionPoint;
        public Transform InteractionPoint => _interactionPoint;

        [SerializeField]
        private float _interactionDelay = .4f;

        private float _lastInteractedTime;
        
        public virtual bool CanInteract()
        {
            if (Time.time <= _lastInteractedTime + _interactionDelay)
                return false;

            return true;
        }

        public void Interact()
        {
            _lastInteractedTime = Time.time;
            OnInteract();
        }
        
        protected abstract void OnInteract();
    }
}