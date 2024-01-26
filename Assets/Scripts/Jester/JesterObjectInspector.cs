#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Jestering.Editor
{
    [CustomEditor(typeof(JesterObject))]
    public class JesterObjectInspector : UnityEditor.Editor
    {
        private JesterObject _jesterObject;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(10);

            if (Application.isPlaying)
                return;

            var jesterObject = target as JesterObject;
            if (!jesterObject)
                return;
            
            if (GUILayout.Button("Add and Set Attachment Slots"))
            {
                var pointsParent = FindOrSetPointParent(jesterObject);

                var headSlot = jesterObject.Slots.HeadSlot;
                if (!headSlot || !headSlot.transform)
                {
                    var instantiatedAttachmentSlot = InstantiateAttachmentSlot(pointsParent, "Head Attachment Slot");
                    
                    jesterObject.Slots.HeadSlot = instantiatedAttachmentSlot;
                }

                var faceSlot = jesterObject.Slots.FaceSlot;
                if (!faceSlot || !faceSlot.transform)
                {
                    var instantiatedAttachmentSlot = InstantiateAttachmentSlot(pointsParent, "Face Attachment Slot");
                    
                    jesterObject.Slots.FaceSlot = instantiatedAttachmentSlot;
                }

                var leftArmSlot = jesterObject.Slots.LeftArmSlot;
                if (!leftArmSlot || !leftArmSlot.transform)
                {
                    var instantiatedAttachmentSlot =
                        InstantiateAttachmentSlot(pointsParent, "Left Arm Attachment Slot");
                    
                    jesterObject.Slots.LeftArmSlot = instantiatedAttachmentSlot;
                }

                var rightArmSlot = jesterObject.Slots.RightArmSlot;
                if (!rightArmSlot || !rightArmSlot.transform)
                {
                    var instantiatedAttachmentSlot =
                        InstantiateAttachmentSlot(pointsParent, "Right Arm Attachment Slot");
                    
                    jesterObject.Slots.RightArmSlot = instantiatedAttachmentSlot;
                }
            }
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Reset Attached Objects"))
            {
               jesterObject.ResetAttachments();
            }
            
            GUILayout.BeginHorizontal();
            
            var jesterObj = EditorGUILayout.ObjectField(_jesterObject, typeof(JesterObject)) as JesterObject;
            _jesterObject = jesterObj;

            if (GUILayout.Button("Add to jester object"))
            {
                if(!_jesterObject || _jesterObject.IsAttached)
                    return;
                
                jesterObject.AttachToMe(_jesterObject);
            }

            GUILayout.EndHorizontal();
        }

        private Transform FindOrSetPointParent(JesterObject jo)
        {
            var existingParent = jo.transform.Find("AttachmentSlotParent");
            if (!existingParent)
            {
                existingParent = new GameObject("AttachmentSlotParent").transform;
                existingParent.SetParent(jo.transform, false);
            }

            jo.Slots.parent = existingParent;
            return existingParent;
        }

        private AttachmentSlot InstantiateAttachmentSlot(Transform parent, string name)
        {
            var slotObjectTransform = parent.transform.Find(name);
            if (!slotObjectTransform)
            {
                slotObjectTransform = new GameObject(name).transform;
                slotObjectTransform.SetParent(parent, false);
            }

            if (!slotObjectTransform.TryGetComponent(typeof(AttachmentSlot), out var foundComponent))
            {
                slotObjectTransform.AddComponent<AttachmentSlot>();
            }
            
            return slotObjectTransform.GetComponent<AttachmentSlot>();
        }
    }
}
#endif