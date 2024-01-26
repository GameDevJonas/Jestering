using System;
using UnityEngine;

namespace Jestering
{
    [Serializable]
    public class AttachmentSlot : MonoBehaviour
    {
        public JesterObject attachedJesterObject;

        public void Attach(JesterObject jesterObject)
        {
            attachedJesterObject = jesterObject;
            jesterObject.SetAttachedSlot(this);
            jesterObject.transform.SetParent(transform, false);
            jesterObject.transform.position = transform.position;
        }

        public void ResetSlot()
        {
            if(!attachedJesterObject)
                return;
            
            attachedJesterObject.SetAttachedSlot(null);
            attachedJesterObject.transform.SetParent(null);
            attachedJesterObject = null;
        }
    }
}