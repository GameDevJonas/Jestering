using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jestering
{
    public class JesterObject : MonoBehaviour
    {
        public enum ItemCategory
        {
            Girlypop,
            Obscure,
            Koselig,
            Cute,
            Provocative,
            ComedyGold
        }

        [SerializeField]
        private ItemCategory _category;

        public ItemCategory Category => _category;

        
        private AttachmentSlot _attachedSlot;
        public bool IsAttached => _attachedSlot != null;
        
        [Serializable]
        public struct AttachmentSlots
        {
            [HideInInspector]
            public Transform parent;
            
            public AttachmentSlot HeadSlot;
            public AttachmentSlot FaceSlot;
            public AttachmentSlot LeftArmSlot;
            public AttachmentSlot RightArmSlot;
        }

        [FormerlySerializedAs("_slots")] [SerializeField]
        public AttachmentSlots Slots;

        public void SetAttachedSlot(AttachmentSlot slot)
        {
            _attachedSlot = slot;
        }
        
        public void AttachToMe(JesterObject jesterObject)
        {
            AttachmentSlot slot = FindAvailableSlot();
            if(!slot)
                return;
            
            slot.Attach(jesterObject);
        }

        public void ResetAttachments()
        {
            Slots.HeadSlot.ResetSlot();
            Slots.FaceSlot.ResetSlot();
            Slots.LeftArmSlot.ResetSlot();
            Slots.RightArmSlot.ResetSlot();
        }
        
        private AttachmentSlot FindAvailableSlot()
        {
            if (!Slots.HeadSlot.attachedJesterObject)
                return Slots.HeadSlot;

            if (!Slots.FaceSlot.attachedJesterObject)
                return Slots.FaceSlot;
            
            if (!Slots.LeftArmSlot.attachedJesterObject)
                return Slots.LeftArmSlot;
            
            if (!Slots.RightArmSlot.attachedJesterObject)
                return Slots.RightArmSlot;

            return null;
        }
    }
}